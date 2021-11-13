using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using LTABase.DesignPattern;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerSlotInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] Image masterImage;

    public Player info;

    private void Start()
    {
        Observer.Instance.AddObserver(ObserverNotyName.OnPlayerLefRoom, OnPlayerLeftRoom);
    }
    void OnPlayerLeftRoom(object data)
    {
        Player otherPlayer = (Player)data;
        if(info == otherPlayer)
        {
            Destroy(gameObject);
        }
    }
    public void SetUpInfo(Player _info)
    {
        info = _info;
        if(_info.IsMasterClient) masterImage.enabled = true;
        else masterImage.enabled = false;
        
        if(_info.NickName == PhotonNetwork.LocalPlayer.NickName) GetComponentInParent<Image>().color = Color.red;
        playerName.text = _info.NickName;
    }
    private void OnDestroy()
    {
        Observer.Instance.RemoveObserver(ObserverNotyName.OnPlayerLefRoom, OnPlayerLeftRoom);
    }
}
