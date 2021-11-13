using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

public class PopUpFindRoom : BasePopupInScene
{
    [SerializeField] Transform roomListParent;
    public List<RoomSlotInfo> listRoomInfo = new List<RoomSlotInfo>();

    public void UpdateRoomState(string roomName, bool isOpen, int playerAmount)
    {
        listRoomInfo.ForEach(room =>{
            if(room.info.Name == roomName) room.SetStateRoom(isOpen, playerAmount);
        });
    }

    public void ClearListRoom()
    {
        roomListParent.DetachChildren();
        listRoomInfo.Clear();
    }
    public void LoadNewListRoom(List<RoomInfo> listRoom)
    {
        foreach (RoomInfo info in listRoom)
        {
            if(info.RemovedFromList)
            {
                int index = listRoomInfo.FindIndex(x => x.info.Name == info.Name);
                if(index != -1)
                {
                    Destroy(listRoomInfo[index].gameObject);
                    listRoomInfo.RemoveAt(index);
                }
            }
            else
            {
                int index = listRoomInfo.FindIndex(x => x.info.Name == info.Name);
                if (index == -1)
                {
                    RoomSlotInfo room = CreateController.Instance.CreateRoom(roomListParent);
                    if (room != null)
                    {
                        room.SetRoomInfo(info);
                        listRoomInfo.Add(room);
                    }
                }
            }
        }
    }
}
