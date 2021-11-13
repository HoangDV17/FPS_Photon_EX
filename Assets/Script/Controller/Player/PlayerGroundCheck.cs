using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    PlayerController controller;
    private void Awake()
    {
        controller = GetComponentInParent<PlayerController>();
    }
    //TRigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == controller.gameObject) return;
        controller.SetGroundedState(true);
        //Debug.Log("Ground");
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == controller.gameObject) return;
        controller.SetGroundedState(false);
        //Debug.Log("Ground");
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == controller.gameObject) return;
        controller.SetGroundedState(true);
        //Debug.Log("Ground");
    }
    //Collision
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == controller.gameObject) return;
        controller.SetGroundedState(true);
        //Debug.Log("Ground");
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == controller.gameObject) return;
        controller.SetGroundedState(false);
        //Debug.Log("Ground");
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject == controller.gameObject) return;
        controller.SetGroundedState(true);
        //Debug.Log("Ground");
    }
}
