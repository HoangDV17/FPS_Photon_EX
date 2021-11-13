using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;
using LTABase.DesignPattern;
using UnityBase.Base.UI.Effect;

public class PLayerMelee
{
    public string ID;
    public string playerName;
    public int score;

    public PLayerMelee(string id, string name, int _score)
    {
        ID = id; playerName = name; score = _score;
    }
}
public class MeleeController : MonoBehaviourPunCallbacks
{
    public List<PLayerMelee> listMelee = new List<PLayerMelee>();
    public static MeleeController instance;
    [SerializeField] Transform parrent, canvas;

    PhotonView PV;

    private void Awake()
    {
        // if(instance)
        // {
        //     Destroy(gameObject);
        //     return;
        // }
        // DontDestroyOnLoad(gameObject);
        instance = this;
        PV = GetComponent<PhotonView>();
    }
    private void Start()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            AddPlayerMelee(PhotonNetwork.PlayerList[i].UserId, PhotonNetwork.PlayerList[i].NickName);
        }
    }

    private void NetWorking_EventReceived(EventData obj)
    {
        if (obj.Code == RAISEVENTCODE.EVENTCODEDIE)
        {
            object[] data = (object[])obj.CustomData;
            //string killerName = null, playerSlain = null;
            Debug.Log("view ID :" + (string)data[1]);
            Debug.Log("player count " + listMelee.Count);
            Debug.Log("player :" + (string)data[0] + " " + " kill " + (string)data[1]);
            //Debug.Log("player slain: " + );
            listMelee.ForEach(melee =>
            {
                if ((string)data[0] == melee.playerName)
                {
                    if ((string)data[1] != (string)data[0]) melee.score += 1;
                    //killerName = melee.playerName;
                    
                }
            });
            
            if ((string)data[0] == (string)data[1] && (string)data[2] == (KillType.bomb).ToString())
            {
                PV.RPC("CreateKillMess", RpcTarget.All, (string)data[1], null, KillType.suicide);
            }
            else
            {
                KillType type = KillType.normal;
                Debug.Log((string)data[2]);
                foreach (KillType killType in (KillType[])KillType.GetValues(typeof(KillType)))
                {
                    if ((string)data[2] == killType.ToString())
                    {
                        type = killType;
                    }
                }
                PV.RPC("CreateKillMess", RpcTarget.All, (string)data[0], (string)data[1], type);
                Dictionary<int, object[]> dic_data = new Dictionary<int, object[]>();
                object[] playerName = new object[listMelee.Count];
                object[] score = new object[listMelee.Count];
                object[] _id = new object[listMelee.Count];

                for (int i = 0; i < listMelee.Count; i++)
                {
                    playerName[i] = listMelee[i].playerName;
                    score[i] = listMelee[i].score;
                    _id[i] = listMelee[i].ID;
                }
                dic_data.Add(0, playerName); dic_data.Add(1, score); dic_data.Add(2, _id);
                Observer.Instance.Notify<Dictionary<int, object[]>>("UpdateRank", dic_data);
                PhotonNetwork.RaiseEvent(RAISEVENTCODE.EVENTCODEUPDATERANK, dic_data, RaiseEventOptions.Default, SendOptions.SendUnreliable);
            }
        }
        else if (obj.Code == RAISEVENTCODE.EVENTCODE_PLAYER_JOIN_ROOM)
        {

        }

        else if (obj.Code == RAISEVENTCODE.EVENTCODEUPDATERANK)
        {
            Dictionary<int, object[]> _data = (Dictionary<int, object[]>)(obj.CustomData);
            object[] P_name = (object[])_data[0];
            object[] P_score = (object[])_data[1];
            object[] P_id = (object[])_data[2];

            for (int i = 0; i < P_name.Length; i++)
            {
                listMelee.ForEach(melee =>
            {
                if ((string)P_name[i] == melee.playerName)
                {
                    melee.score = (int)P_score[i];
                    melee.playerName = P_name[i].ToString();
                    melee.ID = (string)P_id[i];
                    //Debug.Log("player :" + (string)P_name[i] + " " + melee.score + " kill");
                }
            });
            }
            Dictionary<int, object[]> dic_data = new Dictionary<int, object[]>();
            object[] playerName = new object[listMelee.Count];
            object[] score = new object[listMelee.Count];
            object[] _id = new object[listMelee.Count];
            dic_data.Add(0, playerName); dic_data.Add(1, score); dic_data.Add(2, _id);
            for (int i = 0; i < listMelee.Count; i++)
            {
                playerName[i] = listMelee[i].playerName;
                score[i] = listMelee[i].score;
                _id[i] = listMelee[i].ID;
            }
            Observer.Instance.Notify<Dictionary<int, object[]>>("UpdateRank", dic_data);
        }

    }
    [PunRPC]
    void CreateKillMess(string killerName, string playerSlain, KillType type)
    {
        TableCountSlot tableSlot = CreateController.Instance.CreateTableCountSlot(canvas);
        tableSlot.SetContent(killerName, type, playerSlain);
        tableSlot.GetComponent<EffectSlide>().AddShowEndEvent(() =>
        {
            tableSlot.transform.SetParent(parrent);
            StartCoroutine(tableSlot.TimeLife(3f));
        });
    }
    public void AddPlayerMelee(string _id, string _name)
    {
        listMelee.ForEach(melee => { if (_id == melee.ID) return; });
        PLayerMelee pLayerMelee = new PLayerMelee(_id, _name, 0);
        listMelee.Add(pLayerMelee);
        Debug.Log(_id);
        Observer.Instance.Notify<PLayerMelee>("CreateListPlayer", pLayerMelee);
    }
    public void RemovePlayer(Player[] listPlayer)
    {
        List<PLayerMelee> newListMelee = new List<PLayerMelee>();
        Debug.Log("Remove player");
        for (int i = 0; i < listPlayer.Length; i++)
        {
            listMelee.ForEach(melee =>
            {
                if (melee.playerName == listPlayer[i].NickName)
                {
                    PLayerMelee player = new PLayerMelee(listPlayer[i].UserId, listPlayer[i].NickName, melee.score);
                    newListMelee.Add(player);
                }
            });
            //PLayerMelee melee = new PLayerMelee((string)listPlayer[i].UserId, P_name[i].ToString(), (int)P_score[i]);
            //listMelee.Add(melee);
        }
        listMelee = newListMelee;

        Dictionary<int, object[]> dic_data = new Dictionary<int, object[]>();
        object[] playerName = new object[listMelee.Count];
        object[] score = new object[listMelee.Count];
        object[] _id = new object[listMelee.Count];

        for (int i = 0; i < listMelee.Count; i++)
        {
            playerName[i] = listMelee[i].playerName;
            score[i] = listMelee[i].score;
            _id[i] = listMelee[i].ID;
        }
        dic_data.Add(0, playerName); dic_data.Add(1, score); dic_data.Add(2, _id);

        Observer.Instance.Notify<Dictionary<int, object[]>>("PlayerLeftRoom", dic_data);
        // PhotonNetwork.RaiseEvent(RAISEVENTCODE.EVENT_PLAYER_LEFTROOM, dic_data, RaiseEventOptions.Default, SendOptions.SendUnreliable);
    }
    public override void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetWorking_EventReceived;
    }
    public override void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetWorking_EventReceived;
    }
}
