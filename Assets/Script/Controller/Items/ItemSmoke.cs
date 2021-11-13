using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ItemSmoke : ItemBomb
{
    public override void Use()
    {
        if(amount > 0) PV.RPC("BombShoot", RpcTarget.All);
    }

    protected virtual void Update()
    {
        if (delayShoot >= 0) delayShoot -= Time.deltaTime;
        if (timeShoot >= 0)
        {
            timeShoot -= Time.deltaTime;
            if (timeShoot < 0 && isUsed && PV.IsMine)
            {
                // flashBang = CreateController.Instance.CreateFlashBang(transform.position, ((BombInfo)itemInfo).damage, controller.playerManager.PV.ViewID);
                // flashBang.transform.SetParent(transform);
                // flashBang.Bang(controller.playerManager.PV.ViewID, ((BombInfo)itemInfo).damage);
                PV.RPC("BombBang", RpcTarget.All);
            }
        }
        if (isUsed && Input.GetMouseButtonUp(0))
        {
            if (delayShoot <= 0 && PV.IsMine)
            {
                if (PV.IsMine) dir = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f)).direction;
                PV.RPC("BombAddForce", RpcTarget.All, timeShoot, dir);
            }
        }

        if (delayShoot <= 0 && !Input.GetMouseButton(0))
        {
            if (isUsed && PV.IsMine)
            {
                if (PV.IsMine) dir = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f)).direction;
                PV.RPC("BombAddForce", RpcTarget.All, timeShoot, dir);
            }
        }
    }
    [PunRPC]
    void BombBang()
    {
        bombController = CreateController.Instance.CreateGenadeSmoke(transform.position, ((BombInfo)itemInfo).damage);
        bombController.transform.SetParent(transform);
        bombController.Bang(GlobalParameter.playerName, ((BombInfo)itemInfo).damage);
    }
    [PunRPC]
    void BombAddForce(float time, Vector3 dir)
    {
        //if (PV.IsMine)
        //{
            isUsed = false;
            animator.SetBool("aim", isUsed);
            bombController = CreateController.Instance.CreateGenadeSmoke(transform.position, ((BombInfo)itemInfo).damage);
            bombController.transform.SetParent(transform);
            StartCoroutine(bombController.BangCooldown(time));
            if (bombController != null)
            {
                bombController.transform.parent = null;
                bombController.AddForce(dir);
                if(amount <= 0)controller.GetComponent<PlayerEquipItem>().EquipGun(0);
            }
        //}

    }
}
