using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Bullet
{
    public float time;
    public Vector3 initialPosition;
    public Vector3 initialVelocity;
    public TrailRenderer tracer;
}
public class SingleShotGun : ItemGun
{
    protected Ray ray;
    protected RaycastHit hitInfo;
    
    public override void Use()
    {
        currentBullet = ((GunInfo)itemInfo).currentBullet;
        Debug.Log("Use item " + itemInfo.itemName);
        if (delayShoot <= 0 && !isReload) Shoot();
    }
    private void Update()
    {

        if (timeReload > 0)
        {
            timeReload -= Time.deltaTime;
            if (timeReload <= 0 && isReload)
            {
                delayShoot = ((GunInfo)itemInfo).delayShoot;
                int bulletNeedReload =  ((GunInfo)itemInfo).bulletOnMag - currentBullet;

                if(bulletNeedReload + _bulletAmount >= ((GunInfo)itemInfo).bulletOnMag)
                {
                    currentBullet = ((GunInfo)itemInfo).bulletOnMag;
                    ((GunInfo)itemInfo).currentBullet = ((GunInfo)itemInfo).bulletOnMag;
                    _bulletAmount -= bulletNeedReload;
                }
                else
                {
                     currentBullet = bulletNeedReload + _bulletAmount;
                     ((GunInfo)itemInfo).currentBullet = bulletNeedReload + _bulletAmount;
                    _bulletAmount = 0;
                }
                isReload = false;
                animator.SetBool("isReload", isReload);
            }
        }
        if (delayShoot > 0)
        {
            delayShoot -= Time.deltaTime;
            if (((GunInfo)itemInfo).oneShootUnScoped && PV.IsMine) OnUnScoped();
        }
        UpdateBullet(Time.deltaTime);
    }
    public override void Scoped()
    {
        if (!((GunInfo)itemInfo).canScoped) return;
        if (!isReload && timeShoot <= 0)
        {
            isScoped = !isScoped;
            animator.SetBool("isScoped", isScoped);

            if (isScoped) StartCoroutine(OnScoped(0.1f));
            else OnUnScoped();
        }
    }
    IEnumerator OnScoped(float time)
    {
        yield return new WaitForSeconds(time);
        //normalFOV = cam.fieldOfView;
        cam.fieldOfView = ((GunInfo)itemInfo).scopedFOV;
        weaponCamera.SetActive(false);
        scopedOverlay.SetActive(true);
        crossHair.SetActive(false);
    }
    void OnUnScoped()
    {
        isScoped = false;
        animator.SetBool("isScoped", isScoped);
        cam.fieldOfView = normalFOV;
        weaponCamera.SetActive(true);
        scopedOverlay.SetActive(false);
        crossHair.SetActive(true);
    }
    void Shoot()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;
        
        if (currentBullet >= 0)
        {
            currentBullet -= 1;
            ((GunInfo)itemInfo).currentBullet = currentBullet;
        }
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            switch(hit.collider.gameObject.tag)
            {
                case "Head":
                hit.collider.gameObject.GetComponentInParent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage, GlobalParameter.playerName, KillType.headshot);
                break;
                default:
                hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage/2, GlobalParameter.playerName, KillType.normal);
                break;
            }
            
            PV.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal, currentBullet, shootPoint.position);
        }
        else
        {
            PV.RPC("RPC_Shoot", RpcTarget.All, cam.transform.GetChild(0).position, cam.transform.GetChild(0).forward, currentBullet, shootPoint.position);
        }
    }
    [PunRPC]
    void RPC_Shoot(Vector3 point, Vector3 hitNormal, int _currentBullet, Vector3 shootPoint)
    {
        //Collider[] colliders = Physics.OverlapSphere(point, 0.1f);

        if (_currentBullet >= 0)
        {
            animator.SetTrigger("shoot");
            
            if (_currentBullet == 0 && _bulletAmount > 0) Reload();
            if (PV.IsMine)
            {
                StartCoroutine(cam.GetComponent<CameraShaker>().ShootSkake(0.15f, ((GunInfo)itemInfo).stability));
            }
            delayShoot = ((GunInfo)itemInfo).delayShoot;
            ShootFlash();
            //StartCoroutine(CreateShell(((GunInfo)itemInfo).delayShoot * 0.75f));
             AudioManager.Instance.Play(((GunInfo)itemInfo).name + "_Shoot");
            float distance = Vector3.Distance(transform.position, shootPoint);
            
            if(distance <= AudioManager.Instance.GetComponent<AudioSource>().maxDistance)
            {
                
            }
            if (Physics.OverlapSphere(shootPoint, 0.05f).Length <= 0)
            {
                Vector3 velocity = (point - shootPoint).normalized * ((GunInfo)itemInfo).bulletSpeed;
                var bullet = CreateController.Instance.CreateBullet(shootPoint, velocity);
                bullets.Add(bullet);
            }
            //if (colliders.Length != 0)
            //{

            //}
        }
        else
        {
            if(_bulletAmount > 0) Reload();
        }
    }

    public override void Reload()
    {
        StartCoroutine(DelayReload(((GunInfo)itemInfo).delayShoot));
        isReload = true;
        animator.SetBool("isReload", isReload);
    }
    
    IEnumerator DelayReload(float time)
    {
        yield return new WaitForSeconds(time);
        timeReload = ((GunInfo)itemInfo).timeReload;
        animator.SetTrigger("reload");
        if(((GunInfo)itemInfo).canScoped && isScoped && PV.IsMine) 
        { OnUnScoped(); isScoped = false; animator.SetBool("isScoped", isScoped);}
    }
    void ShootFlash()
    {
        foreach (ParticleSystem particle in muzzleFlashs)
        {
            particle.Emit(1);
        }
    }
    public void UpdateBullet(float deltaTime)
    {
        SimulateBullet(deltaTime);
        DestroyBullet();
    }
    void DestroyBullet()
    {
        bullets.RemoveAll(bullet => bullet.time >= ((GunInfo)itemInfo).maxTimeLife);
    }
    void SimulateBullet(float deltaTime)
    {
        if (bullets.Count <= 0) return;
        bullets.ForEach(bullet =>
        {
            Vector3 p0 = GetPosBullet(bullet);
            bullet.time += deltaTime;
            Vector3 p1 = GetPosBullet(bullet);
            RaycastSegment(p0, p1, bullet);
        });
    }
    void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet)
    {
        Vector3 dir = end - start;
        float distance = dir.magnitude;
        ray.origin = start;
        ray.direction = dir;
        if (Physics.Raycast(ray, out hitInfo, distance))
        {
            //Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 1f);
            //HitParticle(hitInfo);
            bullet.tracer.transform.position = hitInfo.point;
            bullet.time = ((GunInfo)itemInfo).maxTimeLife;
            PV.RPC("CreateBulletImpact", RpcTarget.All, hitInfo.point, hitInfo.normal);
        }
        else
        {
            bullet.tracer.transform.position = end;
        }
    }
    [PunRPC]
    void CreateBulletImpact(Vector3 point, Vector3 hitNormal)
    {
        Collider[] colliders = Physics.OverlapSphere(point, 0.1f);
        if (colliders.Length != 0)
        {
            GameObject bulletImpactOBJ = null;
            switch(colliders[0].tag)
            {
                case "Player":
                bulletImpactOBJ =  CreateController.Instance.CreateFleshImpact(point, hitNormal);
                AudioManager.Instance.Play("Flesh_Effect");
                break;

                default:
                bulletImpactOBJ =  CreateController.Instance.CreateMetalImpact(point, hitNormal);
                AudioManager.Instance.Play("Metal_Impact");
                break;
            }
           
            Destroy(bulletImpactOBJ, 5f);
            bulletImpactOBJ.transform.SetParent(colliders[0].transform);
        }

    }
    Vector3 GetPosBullet(Bullet bullet)
    {
        Vector3 gravity = Vector3.down * ((GunInfo)itemInfo).bulletDrop;
        return bullet.initialPosition + (bullet.initialVelocity * bullet.time) + (0.5f * gravity * bullet.time * bullet.time);
    }
}
