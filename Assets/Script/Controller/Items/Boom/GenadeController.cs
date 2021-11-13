using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GenadeController : BoomController
{
    AudioSource explosionSource;
    string id;
    [SerializeField] AudioClip explosionClip;

    protected override void Awake()
    {
        base.Awake();
        explosionSource = GetComponent<AudioSource>();
    }
    public override void Bang(string ID, int damage)
    {
        CreateExplosions();
        Collider[] colliders = Physics.OverlapSphere(transform.position, range);
        if(colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length ; i++)
            {
                colliders[i].GetComponent<IBoomEffect>()?.GenadeEffect();
                colliders[i].GetComponent<IDamageable>()?.TakeDamage(damage, id, KillType.bomb);
                Debug.Log(GlobalParameter.playerName);
            }
        }

        Destroy(gameObject, 3f);
    }
    public void SetActorUse(string name)
    {
        id = name;
    }
    void CreateExplosions()
    {
        GameObject explosions = CreateController.Instance.CreateExplosionsGenade(transform.position);
        if(GameRes.SoundOn) explosionSource.PlayOneShot(explosionClip);
        Destroy(explosions, 2f);
    }
}
