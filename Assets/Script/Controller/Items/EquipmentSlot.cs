using System.Collections;
using System.Collections.Generic;
using UnityBase.Base.UI;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    public WeaponType weaponType;
    public ItemInfo itemInfo;
    [SerializeField] Image checkIcon;

    [SerializeField] ButtonController btnEquip;
    private void Start()
    {
        btnEquip = GetComponent<ButtonController>();
        btnEquip.OnClick(btn => EquipItem());
    }
    public void EquipItem()
    {
        EquipmentManager.instance.EquipItem(this);
    }
    public void CheckIcon(bool isEnabled)
    {
        checkIcon.enabled = isEnabled;
    }
}
