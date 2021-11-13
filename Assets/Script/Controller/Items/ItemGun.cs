using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public abstract class ItemGun : Item
{
    [SerializeField] protected Camera cam;
    [SerializeField] protected GameObject weaponCamera, scopedOverlay, crossHair;
    [SerializeField] protected Animator animator;

    [SerializeField] protected GunShell shellPrefab;
    [SerializeField] protected ParticleSystem[] muzzleFlashs;
    protected List<Bullet> bullets = new List<Bullet>();

    public Transform shootPoint, shellPoint;
    protected float delayShoot = 0, timeReload = -1, timeShoot, normalFOV = 60f, hitViewId;
    public int currentBullet = 0, _bulletAmount = 0;
    protected PhotonView PV;
    protected AnimatorOverrideController overrideController;
    public bool isScoped = false, isReload = false;
    public int bulletOnMag { get { return ((GunInfo)itemInfo).bulletOnMag; } }
    public int bulletAmount { get { return _bulletAmount; } }
    protected virtual void Awake()
    {
        currentBullet = ((GunInfo)itemInfo).bulletOnMag;
        _bulletAmount = ((GunInfo)itemInfo).bulletAmount;
        ((GunInfo)itemInfo).currentBullet = currentBullet;
        overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = overrideController;
    }

    protected virtual void Start()
    {
        
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            //graphic.layer = LayerMask.NameToLayer("weapons");
            weaponCamera.GetComponent<Camera>().cullingMask = LayerMask.GetMask("weapons");
            foreach (Transform trans in graphic.GetComponentsInChildren<Transform>())
            {
                trans.gameObject.layer = LayerMask.NameToLayer("weapons");
            }
        }
    }
    public virtual void CreateShell()
    {
        if (shellPrefab)
        {
            GunShell shell = CreateController.Instance.CreateShell(shellPoint.position, shellPrefab, shellPoint.rotation);
            shell.SetForce();
            Destroy(shell.gameObject, 10f);
        }
    }
    public abstract override void Use();

    public void SwitchCrossHair(Image crossHair)
    {
        crossHair.sprite = ((GunInfo)itemInfo).crosshair;
    }
    public virtual void Scoped()
    {
         Debug.Log("Scoped");
    }
    public virtual void Reload()
    {
        Debug.Log("Reload");
    }
    public void StopReload()
    {
        if (isReload)
        {
            isReload = false;
            timeReload = -1f;
        }
    }
}
