using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class FlashBangController : BoomController
{
    AudioSource explosionSource;
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
                colliders[i].GetComponent<IBoomEffect>()?.FlashEffect();
            }
        }
        
        Destroy(gameObject, 2f);
    }
    void CreateExplosions()
    {
        GameObject explosions = CreateController.Instance.CreateExplosionsFlash(transform.position);
        if(GameRes.SoundOn) explosionSource.PlayOneShot(explosionClip);
        Destroy(explosions, 2f);
    }
}
