using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityBase.Base.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;
using UnityBase.Base.UI.Effect;
using TMPro;
public static class GameRes
{
    public static bool SoundOn
    {
        get => PlayerPrefs.GetInt("SoundOn", 1) == 1;
        set => PlayerPrefs.SetInt("SoundOn", value ? 1 : 0);
    }
}
public class GameSettingPopup : MonoBehaviourPun
{
    public static GameSettingPopup instance;
    [SerializeField] TextMeshProUGUI SoundState;
    [SerializeField] AudioSource bgMusic;
    [SerializeField] ButtonController btnExitGAme, btnSetting, soundBtn;
    [SerializeField] BasePopupInScene SettingPopupUI;
    [SerializeField] PopUpYesNo exitGamePopUp;
    public bool isSettingOpen = false;
    private void Awake()
    {
        instance = this;

    }
    void Start()
    {
        if (GameRes.SoundOn) { SoundState.text = "Sound: On"; bgMusic.Play(); }
        else { SoundState.text = "Sound: Off"; bgMusic.Stop(); }
        btnExitGAme.OnClick(btn => { exitGamePopUp.OpenPopUp(); });
        btnSetting.OnClick(btn => SettingPopupUI.OpenPopUp());
        soundBtn.OnClick(btn =>
        {
            SoundBtnEvent();
        });

        SettingPopupUI.OpenPopUpEvent(() => { btnSetting.gameObject.SetActive(false); isSettingOpen = true; });
        SettingPopupUI.ClosePopUpEvent(() =>
        {
            btnSetting.gameObject.SetActive(true);
            exitGamePopUp.ClosePopUp();
            SettingPopupUI.transform.GetChild(0).gameObject.SetActive(true);
            isSettingOpen = false;
        });

        exitGamePopUp.OpenPopUpEvent(() => { SettingPopupUI.transform.GetChild(0).gameObject.SetActive(false); });
        exitGamePopUp.ClosePopUpEvent(() => { SettingPopupUI.transform.GetChild(0).gameObject.SetActive(true); });

        exitGamePopUp.SetPopUp(ExitGame);
    }
    void SoundBtnEvent()
    {
        Debug.Log("audio");
        if (GameRes.SoundOn) GameRes.SoundOn = false;
        else GameRes.SoundOn = true;
        if (GameRes.SoundOn) { SoundState.text = "Sound: On"; bgMusic.Play(); }
        else { SoundState.text = "Sound: Off"; bgMusic.Stop(); }
    }
    private void Update()
    {
        if(isSettingOpen) {Cursor.lockState = CursorLockMode.None; Cursor.visible = true;}
        else {Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false;}
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SettingPopupUI.gameObject.activeInHierarchy) SettingPopupUI.ClosePopUp();
            else SettingPopupUI.OpenPopUp();
        }
    }
    // Update is called once per frame
    public void ExitGame()
    {
        //Destroy(RoomManager.instance.gameObject);
        //Destroy(Networking.instance.gameObject);
        Destroy(EquipmentManager.instance);
       
        
        LoadingController.Instance.ShowEff();
        LoadingController.Instance.loadingUI.AddShowEndEvent(()=>{PhotonNetwork.LeaveRoom();} );
    }
}
