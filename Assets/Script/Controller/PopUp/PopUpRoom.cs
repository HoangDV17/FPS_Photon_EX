using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityBase.Base.UI;
using Photon.Realtime;

public class PopUpRoom : BasePopupInScene
{
    [SerializeField] TextMeshProUGUI roomName;
    public Transform content;
    [SerializeField] ButtonController btnLeaveRoom, btnStart;

    protected override void Start()
    {
        base.Start();
        btnLeaveRoom.OnClick(btn => Networking.instance.LeaveRoom());
        btnStart.OnClick(btn => Networking.instance.StartGame());
    }
    public void SetUpRoom(string _roomName)
    {
        roomName.text = "Room: " + _roomName;
    }
    public void EnableButtonStart(bool isEnable)
    {
        btnStart.gameObject.SetActive(isEnable);
    }
}
