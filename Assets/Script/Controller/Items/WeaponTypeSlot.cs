using System.Collections;
using System.Collections.Generic;
using UnityBase.Base.UI;
using UnityEngine;
using UnityEngine.UI;

public class WeaponTypeSlot : MonoBehaviour
{
    public WeaponType weaponType;
    public Image icon , bg;
    public ButtonController btnOpen;

    public void ChangeColor(Color color)
    {
        bg.color = color;
    }
}
public enum WeaponType{pistol, rifle, sniper, shotgun}
