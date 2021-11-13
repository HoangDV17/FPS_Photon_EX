using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using LTABase.DesignPattern;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;

public class RaiseEventCode
{
    public const byte UpdateRoomState = 0;
}
public class ObserverNotyName
{
    public const string CreateRoom = "CreateRoom";
    public const string OnPlayerLefRoom = "OnPlayerLefRoom";
}

public class Networking : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public static Networking instance;
    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
        instance = this;
        Observer.Instance.AddObserver(ObserverNotyName.CreateRoom, CreateRoom);
    }
    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("Connecting to master");
            PhotonNetwork.GameVersion = "v1";
            PhotonNetwork.AddCallbackTarget(this);
            PhotonNetwork.ConnectUsingSettings();
        }
        //Debug.Log(PhotonNetwork.PhotonServerSettings.AppSettings.Server);
    }

    public override void OnConnected()
    {
        //MenuController.Instance.ShowLoading();
        if(SceneManager.GetActiveScene().buildIndex == LevelLoader.GameScene) 
        {
            SceneManager.LoadScene(LevelLoader.MenuScene);
        }
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        LoadingController.Instance.ShowEff();
        //RoomOptions room = new RoomOptions()
        //{
        //    IsOpen = true,
        //    IsVisible = true,
        //    MaxPlayers = 2
        //};
        //PhotonNetwork.JoinOrCreateRoom("GamePlay", room, TypedLobby.Default);
        if (!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();

        Debug.Log("OnConnectedToMaster");
        //base.OnConnectedToMaster();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("join room");
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (MenuController.UserName == PhotonNetwork.PlayerList[i].NickName
            && PhotonNetwork.PlayerList[i].UserId != PhotonNetwork.LocalPlayer.UserId)
            {
                PhotonNetwork.LeaveRoom();
                Debug.Log("username exist ");
                MenuController.Instance.ShowUsernameExist();
                return;
            }
        }
        MenuController.Instance.ShowRoom();

        ((PopUpRoom)MenuController.Instance.popUpInRoom).SetUpRoom(PhotonNetwork.CurrentRoom.Name);
        Player[] players = PhotonNetwork.PlayerList;

        Transform p = ((PopUpRoom)MenuController.Instance.popUpInRoom).content;

        foreach (Transform tran in p)
        {
            Destroy(tran.gameObject);
        }
        for (int i = 0; i < players.Length; i++)
        {
            CreateController.Instance.CreatePlayerSlot(p).SetUpInfo(players[i]);
        }
        ((PopUpRoom)MenuController.Instance.popUpInRoom).EnableButtonStart(PhotonNetwork.IsMasterClient);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("join room failed " + message);
        MenuController.Instance.ShowJoinRoomFailed();
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("create room failed " + message);
        MenuController.Instance.ShowCreateRoomFailed();
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("On joined Lobby");
        if (MenuController.UserName.Length > 0) PhotonNetwork.NickName = MenuController.UserName;
        else PhotonNetwork.NickName = "Player" + Random.Range(0, 10000).ToString("0000");

        MenuController.Instance.ShowMenu();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        MenuController.Instance.ShowLoading();
        Debug.Log("On Disconnected");
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            if (!roomList[i].IsOpen) ((PopUpFindRoom)MenuController.Instance.popUpFindRoom).UpdateRoomState(roomList[i].Name, roomList[i].IsOpen, roomList[i].PlayerCount);
        }
        ((PopUpFindRoom)MenuController.Instance.popUpFindRoom).LoadNewListRoom(roomList);
        //listRoom = roomList;
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("player left room");
        if(PhotonNetwork.CurrentRoom.IsOpen) Observer.Instance.Notify<Player>(ObserverNotyName.OnPlayerLefRoom, otherPlayer);
    }
    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {

        //Debug.Log(PhotonNetwork.CurrentRoom.CustomProperties["itemIndex"]);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Transform p = ((PopUpRoom)MenuController.Instance.popUpInRoom).content;
        CreateController.Instance.CreatePlayerSlot(p).SetUpInfo(newPlayer);
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if(PhotonNetwork.CurrentRoom.IsOpen)((PopUpRoom)MenuController.Instance.popUpInRoom).EnableButtonStart(PhotonNetwork.IsMasterClient);
    }
    //================================================================================================================//

    void CreateRoom(object data)
    {
        RoomOptions options = new RoomOptions();
        options.IsOpen = true;
        options.MaxPlayers = 5;
        PhotonNetwork.CreateRoom((string)data, options);
        MenuController.Instance.ShowLoading();
    }
    public void JoinRoom(RoomInfo info)
    {
        if (!PhotonNetwork.InRoom)
        {
            PhotonNetwork.JoinRoom(info.Name);
            MenuController.Instance.ShowLoading();
        }
    }
    public void LeaveRoom()
    {
        MenuController.Instance.ShowListRoom();
        ((PopUpFindRoom)MenuController.Instance.popUpFindRoom).ClearListRoom();
        PhotonNetwork.LeaveRoom();
    }

    public void StartGame()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;

        //if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        //{
            MenuController.Instance.StartGame(() =>
            { 
             PhotonNetwork.LoadLevel(LevelLoader.GameScene);
             //photonView.RPC("LoadingScene", RpcTarget.All, LevelLoader.GameScene);
             ;
            });
        //}
        //else
        //{
            
        //}

    }
    [PunRPC]
    void LoadingScene(int index)
    {
        StartCoroutine(LoadScene(index));
    }
    IEnumerator LoadScene(int name)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            Debug.Log(progress);
            yield return null;
        }
    }
    //public void sendEvent()
    //{
    //    //int[] content = new int[2] { index / 8, index % 8 };
    //    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
    //    PhotonNetwork.RaiseEvent(1, "data", raiseEventOptions, SendOptions.SendReliable);
    //}

    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        sendEvent();
    //        Debug.Log("Send Event");
    //    }
    //}

    //public void OnEvent(EventData photonEvent)
    //{
    //    byte EventCode = photonEvent.Code;
    //    if (EventCode == 1)
    //    {
    //        //Dictionary<byte, object> data = photonEvent.Parameters;
    //        //Debug.Log((string)data[0]);
    //    }
    //}
    private void OnDestroy()
    {
        Observer.Instance.RemoveObserver(ObserverNotyName.CreateRoom, CreateRoom);
    }

    public void OnEvent(EventData photonEvent)
    {

    }
}
