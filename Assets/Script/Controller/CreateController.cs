using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;

public class CreateController : MonoBehaviour
{
    public static CreateController Instance;
    [SerializeField] TableCountSlot tableCountSlot;
    [SerializeField] BoomController flashBang, genade, genadeSmoke;
    [SerializeField] RoomSlotInfo room;
    [SerializeField] PlayerSlotInfo playerSlot;
    [SerializeField] GameObject metalImpact, fleshImpact, explosionsFlash, explosionsGenade;
    [SerializeField] TrailRenderer bulletTracer;
    [SerializeField] LineRenderer bulletTrail;
    [SerializeField] ParticleSystem playerDieEffect, bloodEff, smoke;
    private void Awake()
    {
        if (Instance)
        {
           Destroy(gameObject);
           return;
        }
        DontDestroyOnLoad(this);
        Instance = this;
    }
    public TableCountSlot CreateTableCountSlot(Transform parent)
    {
        return Instantiate(tableCountSlot.gameObject, parent).GetComponent<TableCountSlot>();
    }
    public GameObject CreateExplosionsFlash(Vector3 pos)
    {
        return Instantiate(explosionsFlash, pos, Quaternion.identity).GetComponent<GameObject>();
    }
    public ParticleSystem CreateSmoke(Vector3 pos)
    {
        return Instantiate(smoke.gameObject, pos, Quaternion.identity).GetComponent<ParticleSystem>();
    }
    public GameObject CreateExplosionsGenade(Vector3 pos)
    {
        return Instantiate(explosionsGenade, pos, Quaternion.identity).GetComponent<GameObject>();
    }
    public ParticleSystem CreateBlood(Vector3 pos)
    {
        return Instantiate(bloodEff.gameObject, pos, Quaternion.identity).GetComponent<ParticleSystem>();
    }
    public BoomController CreateFlashBang(Vector3 pos, int damage)
    {
        BoomController boom = Instantiate(flashBang, pos, Quaternion.identity).GetComponent<BoomController>();
        boom.damage = damage;
        return boom;
    }
    public BoomController CreateGenade(Vector3 pos, int damage, string name)
    {
        BoomController boom = Instantiate(genade, pos, Quaternion.identity).GetComponent<BoomController>();
        boom.damage = damage;
        boom.GetComponent<GenadeController>().SetActorUse(name);
        return boom;
    }
    public BoomController CreateGenadeSmoke(Vector3 pos, int damage)
    {
        BoomController boom = Instantiate(genadeSmoke, pos, Quaternion.identity).GetComponent<BoomController>();
        boom.damage = damage;
        return boom;
    }
    public ParticleSystem CreatePlayerDieEff(Vector3 pos)
    {
        return Instantiate(playerDieEffect.gameObject, pos, Quaternion.identity).GetComponent<ParticleSystem>();
    }
    public TrailRenderer CreateBulletTracer(Vector3 point)
    {
        return Instantiate(bulletTracer.gameObject, point, Quaternion.identity).GetComponent<TrailRenderer>();
    }
    public LineRenderer CreateBulletTrail(Vector3 point)
    {
        return Instantiate(bulletTrail.gameObject, point, Quaternion.identity).GetComponent<LineRenderer>();
    }
    public GameObject CreateMetalImpact(Vector3 pos, Vector3 hitNormal)
    {
        return Instantiate(metalImpact, pos + hitNormal * 0.0001f, Quaternion.LookRotation(hitNormal, Vector3.up) * metalImpact.transform.rotation);
    }
    public GameObject CreateFleshImpact(Vector3 pos, Vector3 hitNormal)
    {
        return Instantiate(fleshImpact, pos + hitNormal * 0.0001f, Quaternion.LookRotation(hitNormal, Vector3.up) * fleshImpact.transform.rotation);
    }
    public RoomSlotInfo CreateRoom(Transform parent)
    {
        return Instantiate(room.gameObject, parent).GetComponent<RoomSlotInfo>();
    }
    public PlayerSlotInfo CreatePlayerSlot(Transform parent)
    {
        return Instantiate(playerSlot.gameObject, parent).GetComponent<PlayerSlotInfo>();
    }
    public GameObject CreatePlayerManager()
    {
        return PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
    }
    public GameObject CreatePlayerController(PhotonView PV, Vector3 pos, Quaternion quaternion)
    {
        return PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), pos, quaternion, 0, new object[] { PV.ViewID});
    }
    public Bullet CreateBullet(Vector3 pos, Vector3 velocity)
    {
        Bullet bullet = new Bullet();
        bullet.initialVelocity = velocity;
        bullet.initialPosition = pos;
        bullet.time = 0f;
        bullet.tracer = CreateBulletTracer(pos);
        bullet.tracer.AddPosition(pos);
        return bullet;
    }
    public GunShell CreateShell(Vector3 pos, GunShell shellPrefab, Quaternion ro)
    {
        return Instantiate(shellPrefab.gameObject, pos, ro).GetComponent<GunShell>();
    }
}
