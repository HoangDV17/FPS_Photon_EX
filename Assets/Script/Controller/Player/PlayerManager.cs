using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityBase.Base.UI.Effect;

public static class GlobalParameter
{
    public static string playerName;
    public static string playerID;

    public static void SetGlobalValue(string _name, string _id)
    {
        playerName = _name;
        playerID = _id;
    }

}
public class PlayerManager : MonoBehaviourPunCallbacks
{
    [SerializeField] EffectSlide effectSlide;
    public PhotonView PV;
    GameObject controller;
    public static string hitViewID, playerslainID;
    public static KillType _KillType;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            GlobalParameter.SetGlobalValue(PV.Owner.NickName, PV.Owner.UserId);
            Debug.Log(GlobalParameter.playerName);
        }
    }

    public void SetHitViewID(string _id, KillType type)
    {
        hitViewID = _id;
        _KillType = type;
    }
    private void Start()
    {
        if (PV.IsMine)
        {
            CraetePlayerController();

            if (!RoomManager.instance.isLoad)
            {
                LoadingController.Instance.HideEff();
                effectSlide.ShowEffect();
                effectSlide.AddShowEndEvent(() => { StartCoroutine(StartGame(2f)); });
            }
            else
            {
                StartCoroutine(LoadScene());
            }
        }
        else
        {
            Destroy(effectSlide.transform.parent.gameObject);
        }

        //object[] datas = new object[] { PV.Owner.NickName, PV.Owner.UserId};
        //PhotonNetwork.RaiseEvent(RAISEVENTCODE.EVENTCODE_PLAYER_JOIN_ROOM, datas, RaiseEventOptions.Default, SendOptions.SendUnreliable);
        //if(PV.IsMine) MeleeController.instance.AddPlayerMelee(PV.ViewID, PV.Owner.NickName);
    }
    IEnumerator LoadScene()
    {
        while (!RoomManager.instance.isLoad)
        {
            //float progress = Mathf.Clamp01(operation / 0.9f);
            //Debug.Log(progress);
            yield return new WaitForEndOfFrame();
        }
        RoomManager.instance.isLoad = false;
        LoadingController.Instance.HideEff();
        effectSlide.ShowEffect();
        effectSlide.AddShowEndEvent(() => { StartCoroutine(StartGame(2f)); });
    }
    IEnumerator StartGame(float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            effectSlide.GetComponentInParent<CanvasGroup>().alpha = 1 - elapsed / duration;
            yield return null;
        }
        effectSlide.transform.parent.gameObject.SetActive(false);
        effectSlide.HideEffect();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (otherPlayer.NickName != PV.Owner.NickName)
        {
            Debug.Log("Leave room");
            MeleeController.instance.RemovePlayer(PhotonNetwork.PlayerList);
            //PhotonNetwork.RaiseEvent(RAISEVENTCODE.EVENT_PLAYER_LEFTROOM, otherPlayer.NickName, RaiseEventOptions.Default, SendOptions.SendUnreliable);
            ////PhotonNetwork.Destroy(controller);
            //Destroy(gameObject);
        }
        //base.OnPlayerLeftRoom(otherPlayer);
    }

    void CraetePlayerController()
    {
        Transform spawnPoint = SpawnPointManager.Instance.GetSpawnpoint();
        controller = CreateController.Instance.CreatePlayerController(PV, spawnPoint.position, spawnPoint.rotation);
    }

    public void Die()
    {
        Camera.main.transform.position = controller.transform.position;
        Camera.main.transform.rotation = Quaternion.LookRotation(controller.transform.forward);
        PhotonNetwork.Destroy(controller);
        StartCoroutine(DelayRestart());
    }
    IEnumerator DelayRestart()
    {
        yield return new WaitForSeconds(2f);
        CraetePlayerController();
        //PV.RPC("ResetHitID", RpcTarget.All);
        if (!RoomManager.instance.isLoad)
        {
            LoadingController.Instance.HideEff();
        }
    }
}
