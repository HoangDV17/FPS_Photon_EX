using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "FPS/new gun")]
public class GunInfo : ItemInfo
{
    public Sprite crosshair;
    public int currentBullet
    {
        get => PlayerPrefs.GetInt("currentBullet" + itemName, bulletOnMag);
        set => PlayerPrefs.SetInt("currentBullet" + itemName, value);
    }
    public bool canScoped, oneShootUnScoped;
    public int damage, bulletAmount, bulletOnMag;
    public float bulletSpeed, bulletDrop, maxTimeLife, delayShoot, timeReload, delayReload, stability, scopedFOV;
}
