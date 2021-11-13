using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class Item : MonoBehaviourPunCallbacks
{
    public ItemInfo itemInfo;
    public GameObject graphic;
    public abstract void Use();
}
