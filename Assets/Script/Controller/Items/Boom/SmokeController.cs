using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SmokeController : BoomController
{
    [SerializeField] ParticleSystem smoke;
    public override void Bang(string ID, int damage)
    {
       CreateSmoke();
    }
    [PunRPC]
    void CreateSmoke()
    {
        CreateController.Instance.CreateSmoke(transform.position);
        smoke.Play();
        Destroy(gameObject, 10f);
    }
}
