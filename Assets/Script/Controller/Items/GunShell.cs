using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShell : MonoBehaviour
{
    public float magnitude = 2f;
    Rigidbody rigidbody;
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    public void SetForce()
    {
        rigidbody.AddForce(transform.forward * Random.Range(2, 5) * magnitude, ForceMode.Impulse);
    }
}
