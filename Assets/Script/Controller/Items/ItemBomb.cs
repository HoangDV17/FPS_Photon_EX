using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class ItemBomb : Item
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected Camera cam;
    protected BoomController bombController;
    protected bool isUsed = false;
    protected PlayerController controller;
    protected PhotonView PV;
    protected float delayBang = 5f, delayShoot = -1f, timeShoot = 0;
    protected int damage, range, amount;
    public int currentAmount { get { return amount; } }
    protected Vector3 dir;
    private void Awake()
    {
        controller = GetComponentInParent<PlayerController>();
        PV = GetComponent<PhotonView>();
        amount = ((BombInfo)itemInfo).amount;
        //animator = GetComponentInChildren<Animator>();
    }
    public override void Use()
    {
        Debug.Log("use item bomb");
    }
    [PunRPC]
    protected void BombShoot()
    {
        //if (delayShoot <= 0 && PV.IsMine)
        //{
            if (!isUsed) animator.SetTrigger("shoot");
            isUsed = true;
            animator.SetBool("aim", isUsed);
            delayShoot = 0.7f;
            timeShoot = 5f;
            amount -= 1;
        //}
    }
    public void SwitchCrossHair(Image crossHair)
    {
        crossHair.sprite = ((BombInfo)itemInfo).reticle;
    }
}
