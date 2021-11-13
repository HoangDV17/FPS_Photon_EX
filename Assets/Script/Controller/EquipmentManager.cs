using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EquipmentSlotData
{
    public static string pistolSlot
    {
        get => PlayerPrefs.GetString("pistolSlot", "Pistol");
        set => PlayerPrefs.SetString("pistolSlot", value);
    }
    public static string rifleSlot
    {
        get => PlayerPrefs.GetString("rifleSlot", "Rifle");
        set => PlayerPrefs.SetString("rifleSlot", value);
    }
    public static string sniperSlot
    {
        get => PlayerPrefs.GetString("sniperSlot", "Dragunov");
        set => PlayerPrefs.SetString("sniperSlot", value);
    }
    public static string shotGunSlot
    {
        get => PlayerPrefs.GetString("shotGunSlot", "Double Barrel");
        set => PlayerPrefs.SetString("shotGunSlot", value);
    }
}
public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager instance;
    [SerializeField] Transform parentWeaponType, parentListWeapon;
    public ItemInfo[] equipInfos = new ItemInfo[4];
    WeaponTypeSlot[] weaponTypeSlots;

    EquipmentSlot[] equipmentSlots;
    public Transform[] listPopUpWeapons;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        //DontDestroyOnLoad(gameObject);
        instance = this;
        LoadWeapon();
    }
    private void Start()
    {
        weaponTypeSlots[0].btnOpen.OnClick(btn => ShowListWeapon(0));
        weaponTypeSlots[1].btnOpen.OnClick(btn => ShowListWeapon(1));
        weaponTypeSlots[2].btnOpen.OnClick(btn => ShowListWeapon(2));
        weaponTypeSlots[3].btnOpen.OnClick(btn => ShowListWeapon(3));

        ShowListWeapon(0);
    }
    void LoadWeapon()
    {
        equipInfos[0] = Resources.Load<ItemInfo>("Items/" + EquipmentSlotData.pistolSlot);
        equipInfos[1] = Resources.Load<ItemInfo>("Items/" + EquipmentSlotData.rifleSlot);
        equipInfos[2] = Resources.Load<ItemInfo>("Items/" + EquipmentSlotData.sniperSlot);
        equipInfos[3] = Resources.Load<ItemInfo>("Items/" + EquipmentSlotData.shotGunSlot);

        weaponTypeSlots = parentWeaponType.GetComponentsInChildren<WeaponTypeSlot>();
        equipmentSlots = parentListWeapon.GetComponentsInChildren<EquipmentSlot>();
        for (int i = 0; i < listPopUpWeapons.Length; i++)
        {
            listPopUpWeapons[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            for (int j = 0; j < equipInfos.Length; j++)
            {
                if(equipmentSlots[i].itemInfo.itemName == equipInfos[j].itemName)
                {
                    equipmentSlots[i].CheckIcon(true);
                }
            }
        }
    }
    public void ShowListWeapon(int index)
    {
        for (int i = 0; i < listPopUpWeapons.Length; i++)
        {
            listPopUpWeapons[i].gameObject.SetActive(false);
            weaponTypeSlots[i].ChangeColor(Color.black);
        }
        listPopUpWeapons[index].gameObject.SetActive(true);
        weaponTypeSlots[index].ChangeColor(Color.red);
    }
    public void EquipItem(EquipmentSlot slot)
    {
        equipInfos[(int)slot.weaponType] = slot.itemInfo;
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipmentSlots[i].weaponType == slot.weaponType)
            {
                equipmentSlots[i].CheckIcon(false);
                switch(slot.weaponType)
                {
                    case WeaponType.pistol:
                    EquipmentSlotData.pistolSlot = equipmentSlots[i].itemInfo.itemName;
                    break;
                    case WeaponType.rifle:
                    EquipmentSlotData.rifleSlot = equipmentSlots[i].itemInfo.itemName;
                    break;
                    case WeaponType.shotgun:
                    EquipmentSlotData.shotGunSlot = equipmentSlots[i].itemInfo.itemName;
                    break;
                    case WeaponType.sniper:
                    EquipmentSlotData.sniperSlot = equipmentSlots[i].itemInfo.itemName;
                    break;
                }
            }
                
        }
        slot.CheckIcon(true);
        //Debug.Log((int)weaponType);
    }
}
