using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class PlayerEquipItem : MonoBehaviourPunCallbacks
{
    [SerializeField] Item[] itemGuns;
    [SerializeField] Item[] currentEquips;
    [SerializeField] TextMeshProUGUI bulletAmount, boomAmount;
    [SerializeField] Image crossHair;
    public int itemIndex, previousItemIndex = -1;
    bool isScoped = false;
    PhotonView PV;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        //Debug.Log(EquipmentManager.instance.equipInfos.Length);
        currentEquips = new Item[EquipmentManager.instance.equipInfos.Length];

        for (int j = 0; j < itemGuns.Length; j++)
        {
            for (int i = 0; i < EquipmentManager.instance.equipInfos.Length; i++)
            {
                string itemName = EquipmentManager.instance.equipInfos[i].itemName;
                if (itemGuns[j].itemInfo.itemName == itemName)
                {
                    currentEquips[i] = itemGuns[j];
                }
            }
        }

    }

    public ItemGun currentEquip { get { return (ItemGun)itemGuns[itemIndex]; } }
    public void EquipGun(int index)
    {
        int propertyData = index;
        itemIndex = index;
        if (index >= currentEquips.Length) index = currentEquips.Length - 1;
        for (int i = 0; i < itemGuns.Length; i++)
        {
            if (currentEquips[index].itemInfo.itemName == itemGuns[i].itemInfo.itemName)
            {
                itemIndex = i;
            }
        }
        if (itemIndex == previousItemIndex) return;
        if (itemGuns[itemIndex].GetComponent<ItemBomb>() != null)
        {
            if (((ItemBomb)itemGuns[itemIndex]).currentAmount <= 0) return;
        }
        //itemIndex = index;

        itemGuns[itemIndex].graphic.SetActive(true);

        if (previousItemIndex != -1)
        {
            if (itemGuns[previousItemIndex].GetComponent<ItemGun>() != null) ((ItemGun)itemGuns[previousItemIndex]).StopReload();
            itemGuns[previousItemIndex].graphic.SetActive(false);
        }


        previousItemIndex = itemIndex;
        if (PV.IsMine)
        {
            if (itemGuns[itemIndex].GetComponent<ItemGun>() != null)
            {
                ((ItemGun)itemGuns[itemIndex]).SwitchCrossHair(crossHair);
                bulletAmount.transform.parent.gameObject.SetActive(true);
                boomAmount.transform.parent.gameObject.SetActive(false);
            }
            if (itemGuns[itemIndex].GetComponent<ItemKnife>() != null)
            {
                ((ItemKnife)itemGuns[itemIndex]).SwitchCrossHair(crossHair);
                bulletAmount.transform.parent.gameObject.SetActive(false);
                boomAmount.transform.parent.gameObject.SetActive(false);
            }
            if (itemGuns[itemIndex].GetComponent<ItemBomb>() != null)
            {
                ((ItemBomb)itemGuns[itemIndex]).SwitchCrossHair(crossHair);
                bulletAmount.transform.parent.gameObject.SetActive(false);
                boomAmount.transform.parent.gameObject.SetActive(true);

                int _bombAmount = ((ItemBomb)itemGuns[itemIndex]).currentAmount;
                boomAmount.text = "X " + _bombAmount;
            }
            Hashtable hash = new Hashtable();
            hash.Add("itemIndex", propertyData);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            if (itemGuns[itemIndex].GetComponent<ItemGun>() != null)
            {
                if (((ItemGun)itemGuns[itemIndex]).currentBullet <= 0)
                {
                    ItemGun gun = ((ItemGun)itemGuns[itemIndex]);
                    PV.RPC("Reload", RpcTarget.All, gun.currentBullet, gun.bulletOnMag, gun.isReload, gun.bulletAmount);
                }
                if (((ItemGun)itemGuns[itemIndex]).currentBullet >= 0)
                    bulletAmount.text = "X " + ((ItemGun)itemGuns[itemIndex]).currentBullet + "/" + ((ItemGun)itemGuns[itemIndex]).bulletAmount;
                else bulletAmount.text = "X " + 0 + "/" + ((ItemGun)itemGuns[itemIndex]).bulletAmount;
            }
        }

    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!PV.IsMine && targetPlayer == PV.Owner)
        {
            EquipGun((int)changedProps["itemIndex"]);
        }
    }
    public void SwitchGun()
    {
        for (int i = 0; i < currentEquips.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                EquipGun(i); break;
            }
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            if (itemIndex >= currentEquips.Length - 1)
            {
                EquipGun(0);
            }
            else
            {
                EquipGun(itemIndex + 1);
            }
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            if (itemIndex <= 0)
            {
                EquipGun(currentEquips.Length - 1);
            }
            else
            {
                EquipGun(itemIndex - 1);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            itemGuns[itemIndex].Use();
        }
        if (itemGuns[itemIndex].GetComponent<ItemGun>() != null)
        {
            ItemGun gun = ((ItemGun)itemGuns[itemIndex]);
            if (Input.GetMouseButtonDown(1))
            {
                if (!gun.isReload) ((ItemGun)itemGuns[itemIndex]).Scoped();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                PV.RPC("Reload", RpcTarget.All, gun.currentBullet, gun.bulletOnMag, gun.isReload, gun.bulletAmount);
            }
            if (gun.currentBullet >= 0)
                bulletAmount.text = "X " + gun.currentBullet + "/" + gun.bulletAmount;
        }
        if (itemGuns[itemIndex].GetComponent<ItemBomb>() != null)
        {
            int _bombAmount = ((ItemBomb)itemGuns[itemIndex]).currentAmount;
            boomAmount.text = "X " + _bombAmount;
        }
    }
    [PunRPC]
    void Reload(int bullet, int bulletInMag, bool isReload, int bulletAmount)
    {
        if (bullet >= bulletInMag || isReload || bulletAmount <= 0) return;
        ((ItemGun)itemGuns[itemIndex]).Reload();
    }
}
