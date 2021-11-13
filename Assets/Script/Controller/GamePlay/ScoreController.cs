using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using ExitGames.Client.Photon;
using LTABase.DesignPattern;
using Photon.Realtime;
using UnityEngine.UI;

public class ScoreController : MonoBehaviourPunCallbacks
{
    [SerializeField] TextMeshProUGUI score, playerName;
    [SerializeField] GameObject scoreUI, scoreParent, nameParent;
    PhotonView PV;
    TextMeshProUGUI[] names, playerscores;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    void Start()
    {
        //PhotonNetwork.NetworkingClient.EventReceived += NetWorking_Received;
        Observer.Instance.AddObserver("CreateListPlayer", CreateListPlayer);
        Observer.Instance.AddObserver("UpdateRank", UpdateRank);
        Observer.Instance.AddObserver("PlayerLeftRoom", OnPlayerLeftRoom);
        scoreUI.SetActive(false);
    }
    void CreateListPlayer(object data)
    {
        Debug.Log("Create List Player");
        PLayerMelee melee = (PLayerMelee)data;
        Instantiate(score.gameObject, scoreParent.transform).GetComponent<TextMeshProUGUI>().text = melee.score.ToString();
        Instantiate(playerName.gameObject, nameParent.transform).GetComponent<TextMeshProUGUI>().text = melee.playerName;

        names = nameParent.GetComponentsInChildren<TextMeshProUGUI>();
        playerscores = scoreParent.GetComponentsInChildren<TextMeshProUGUI>();

        for (int i = 0; i < names.Length; i++)
        {
            Debug.Log(GlobalParameter.playerName);
            if (names[i].text == GlobalParameter.playerName)
            {
                names[i].GetComponentInChildren<Image>().enabled = true;
            }
            else names[i].GetComponentInChildren<Image>().enabled = false;
        }
    }
    void OnPlayerLeftRoom(object data)
    {
        Dictionary<int, object[]> _data = (Dictionary<int, object[]>)(data);
        object[] P_name = (object[])_data[0];
        object[] P_score = (object[])_data[1];
        object[] P_id = (object[])_data[2];


        for (int i = 0; i < names.Length; i++)
        {
            Destroy(names[i].gameObject);
            Destroy(playerscores[i].gameObject);
        }
        
        for (int i = 0; i < P_name.Length; i++)
        {
            Instantiate(score.gameObject, scoreParent.transform).GetComponent<TextMeshProUGUI>().text = P_score[i].ToString();
            Instantiate(playerName.gameObject, nameParent.transform).GetComponent<TextMeshProUGUI>().text = P_name[i].ToString();
        }
        names = nameParent.GetComponentsInChildren<TextMeshProUGUI>();
        playerscores = scoreParent.GetComponentsInChildren<TextMeshProUGUI>();
    }
    void UpdateRank(object data)
    {
        Dictionary<int, object[]> _data = (Dictionary<int, object[]>)(data);
        object[] P_name = (object[])_data[0];
        object[] P_score = (object[])_data[1];
        object[] P_id = (object[])_data[2];

        //int index = 0;
        for (int i = 0; i < names.Length; i++)
        {
            names[i].text = P_name[i].ToString();
            playerscores[i].text = P_score[i].ToString();
        }
        Sort(playerscores, names);

    }
    public void Sort(TextMeshProUGUI[] array, TextMeshProUGUI[] array1)
    {
        for (int i = 0; i < array.Length - 1; i++)
        {
            int maxIndex = i;
            TextMeshProUGUI melee = array[i];
            for (int j = i + 1; j < array.Length; j++)
            {
                if (int.Parse(array[j].text) > int.Parse(melee.text))
                {
                    maxIndex = j;
                    melee = array[j];
                }
            }
            string temp = array[i].text;
            array[i].text = array[maxIndex].text;
            array[maxIndex].text = temp;

            string temp1 = array1[i].text;
            array1[i].text = array1[maxIndex].text;
            array1[maxIndex].text = temp1;
        }
        for (int i = 0; i < names.Length; i++)
        {
            if (names[i].text == GlobalParameter.playerName)
            {
                names[i].GetComponentInChildren<Image>().enabled = true;
            }
            else names[i].GetComponentInChildren<Image>().enabled = false;
        }
    }
    // private void NetWorking_Received(EventData obj)
    // {
    //     if(obj.Code == 3)
    //     {
    //         Debug.Log("player join room");
    //         Dictionary<int, object> _data = (Dictionary<int, object>)obj.CustomData;
    //         Debug.Log((object[])_data[0]);

    //         //AddPlayerMelee((float)_data[1], (string)_data[0]);
    //         //Instantiate(score.gameObject, scoreParent.transform).GetComponent<TextMeshProUGUI>().text = ((float)_data[1]).ToString();
    //         //    Instantiate(playerName.gameObject, nameParent.transform).GetComponent<TextMeshProUGUI>().text = ((float)_data[0]).ToString();
    //     }
    // }
    // Update is called once per frame
    void Update()
    {
        scoreUI.SetActive(Input.GetKey(KeyCode.Tab));
    }
    private void OnDestroy()
    {
        Observer.Instance.RemoveObserver("CreateListPlayer", CreateListPlayer);
        Observer.Instance.RemoveObserver("UpdateRank", UpdateRank);
        Observer.Instance.RemoveObserver("PlayerLeftRoom", UpdateRank);
    }
}
