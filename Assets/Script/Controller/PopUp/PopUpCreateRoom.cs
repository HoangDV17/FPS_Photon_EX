using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityBase.Base.UI;
using LTABase.DesignPattern;

public class PopUpCreateRoom : BasePopupInScene
{
    [SerializeField] TMP_InputField roomNameInput;
    [SerializeField] ButtonController btnCreateRoom;
    protected override void Start()
    {
        base.Start();
        btnCreateRoom.OnClick(btn => CreateRoom());
    }
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInput.text)) return;
        Observer.Instance.Notify<string>(ObserverNotyName.CreateRoom, roomNameInput.text);
        ClosePopUp();
    }
}
