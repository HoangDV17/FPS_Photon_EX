 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using UnityBase.Base.UI;

public class RoomSlotInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI roomName, playerAmount, roomState;
    public RoomInfo info;
    ButtonController btnJoinRoom;
    private void Awake()
    {
        btnJoinRoom = GetComponent<ButtonController>();
        btnJoinRoom.OnClick(btn => OnClickEvent());
    }
    public void SetStateRoom(bool isOpen, int _playerAmount)
    {
        playerAmount.text = _playerAmount.ToString() + "/" + info.MaxPlayers.ToString();
        if(isOpen) {roomState.text = "In Lobby"; roomState.color = Color.green;}
        else {roomState.text = "In Game"; roomState.color = Color.red;}
    }
    public void SetRoomInfo(RoomInfo _info)
    {
        info = _info;
        roomName.text = _info.Name;
        SetStateRoom(_info.IsOpen, _info.PlayerCount);
        Debug.Log(info.Name);
    }
    void OnClickEvent()
    {
        Networking.instance.JoinRoom(info);
    }
}
