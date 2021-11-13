using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public abstract class BoomController : MonoBehaviour
{
    [SerializeField]
    protected float range;
    public int damage;
    [SerializeField] Rigidbody rb;
    [SerializeField] protected PhotonView PV;
    protected virtual void Awake()
    {
        //rb = GetComponent<Rigidbody>();
        //PV = GetComponent<PhotonView>();
        rb.isKinematic = true;
        foreach (MeshRenderer item in transform.GetComponentsInChildren<MeshRenderer>())
        {
            item.enabled = false;
        }
    }
    public void AddForce(Vector3 dir)
    {
        foreach (MeshRenderer item in transform.GetComponentsInChildren<MeshRenderer>())
        {
            item.enabled = true;
        }
        rb.isKinematic = false;
        rb.AddForce(dir * 30f, ForceMode.VelocityChange);
    }
    public abstract void Bang(string ID, int damage);
    public IEnumerator BangCooldown(float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        Bang(GlobalParameter.playerID, damage);
    }
}
