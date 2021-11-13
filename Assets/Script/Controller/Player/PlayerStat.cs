using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityBase;
using UnityBase.Base.UI.Effect;
using Photon.Realtime;
using ExitGames.Client.Photon;
using LTABase.DesignPattern;

public class RAISEVENTCODE
{
    public const byte EVENTCODEDIE = 1;
    public const byte EVENTCODEUPDATERANK = 2;
    public const byte EVENTCODE_PLAYER_JOIN_ROOM = 3;
    public const byte EVENT_PLAYER_LEFTROOM = 4;
}
public class PlayerStat : MonoBehaviourPunCallbacks, IDamageable
{
    [SerializeField] HPController hPController;
    [SerializeField] CameraShaker cameraShaker;
    [SerializeField] EffectAlphaShow damageOverlay;
    PlayerController playerController;

    public static string hitViewID;
    KillType deadType;
    // int maxHealth = 100;
    // int currentHealth;
    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        hPController.dieEvent += DieEvent;
    }
    private void Update()
    {
        if (transform.position.y < -50f)
        {
            DieEvent();
        }
    }
    public void TakeDamage(int damage, string playerID, KillType killType)
    {
        Debug.Log("Took damage " + damage);
        Debug.Log("id " + playerID);
        Debug.Log(killType);
        hitViewID = playerID;
        deadType = killType;

        playerController.PV.RPC("RPC_TakeDamage", RpcTarget.All, damage, playerID, killType);

    }
    [PunRPC]
    void RPC_TakeDamage(int damage, string _id, KillType type)
    {
        if (!playerController.PV.IsMine) return;
        playerController.playerManager.SetHitViewID(_id, type);
        StartCoroutine(cameraShaker.Skake(0.05f, 0.001f));
        damageOverlay.ShowEffect();
        damageOverlay.AddShowEndEvent(() => { damageOverlay.HideEffect(); });
        hPController.ChangeHP(damage);
    }
    public void DieEvent()
    {
        playerController.PV.RPC("Die", RpcTarget.All);
        string hitName = hitViewID;
        string slainName = PhotonNetwork.LocalPlayer.NickName;
        string killType = deadType.ToString();
        object[] datas = new object[] { hitName, slainName, killType };
        Debug.Log(hitViewID);
        PhotonNetwork.RaiseEvent(RAISEVENTCODE.EVENTCODEDIE, datas, RaiseEventOptions.Default, SendOptions.SendReliable);
        playerController.playerManager.Die();
    }
    [PunRPC]
    void Die()
    {
        CreateController.Instance.CreatePlayerDieEff(transform.position);
    }
}
