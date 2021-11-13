using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "FPS/new bomb")]
public class BombInfo : ItemInfo
{
    public int damage;
    public float range;
    public int amount;

    public Sprite reticle;
}
