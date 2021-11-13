using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityBase.Base.UI;
using UnityBase.Base.UI.Effect;
using UnityEngine;
using UnityEngine.UI;

public class LevelLoader
{
    public const int MenuScene = 0;
    public const int GameScene = 1;
}

public class MenuController : MonoBehaviourPunCallbacks
{
    public static MenuController Instance;
    public static string UserName
    {
        get => PlayerPrefs.GetString("UserName", null);
        set => PlayerPrefs.SetString("UserName", value);
    }
    public EffectAlphaShow MenuUI;
    [SerializeField] InputField inputUserName;
    //[SerializeField] Image loadingBG;
    [SerializeField] ButtonController btnCreateRoom, btnPlay, BtnQuitGame, btnEquip, refreshBtn;
    [SerializeField] GameObject buttonsParent, popUpParent, buttonPos;
    public BasePopupInScene popUpCreateRoom, popUpInRoom, popUpCreateRoomFailed, popUpFindRoom, popJoinRoomFailed, equipPopUp, popUpUsernameExist, popUpStartFailed;
    List<BasePopupInScene> listPopUp = new List<BasePopupInScene>();
    bool isShowMenu = false;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        if (UserName != null) inputUserName.text = UserName;
        MenuUI = GetComponent<EffectAlphaShow>();
        MenuUI.GetComponent<CanvasGroup>().alpha = 0;
        inputUserName.onValueChanged.AddListener(delegate {UsernameChangeCheck();});

        foreach (BasePopupInScene popUp in popUpParent.GetComponentsInChildren<BasePopupInScene>())
        {
            listPopUp.Add(popUp);
        }

        //add button event
        BtnQuitGame.OnClick(btn => Application.Quit());
        btnCreateRoom.OnClick(btn => { popUpCreateRoom.OpenPopUp(); });
        btnPlay.OnClick(btn => { popUpFindRoom.OpenPopUp(); });
        btnEquip.OnClick(btn => equipPopUp.OpenPopUp());
        equipPopUp.ClosePopUp();

        //add pop up create room event
        popUpCreateRoom.OpenPopUpEvent(() => { buttonsParent.SetActive(false); btnCreateRoom.gameObject.SetActive(false); });
        popUpCreateRoom.ClosePopUpEvent(() => { buttonsParent.SetActive(true); btnCreateRoom.gameObject.SetActive(true); });

        //add pop up create room failed
        popUpCreateRoomFailed.OpenPopUpEvent(() => { buttonsParent.SetActive(false); HideLoading(); btnCreateRoom.gameObject.SetActive(false);});
        popUpCreateRoomFailed.ClosePopUpEvent(() => { buttonsParent.SetActive(true); btnCreateRoom.gameObject.SetActive(true);});

        //add pop up find room event
        popUpFindRoom.OpenPopUpEvent(() => { buttonsParent.SetActive(false); });
        popUpFindRoom.ClosePopUpEvent(() => { buttonsParent.SetActive(true); popUpCreateRoom.ClosePopUp(); });

        //add pop up join room failed event
        popJoinRoomFailed.OpenPopUpEvent(() => { buttonsParent.SetActive(false); HideLoading();});
        popJoinRoomFailed.ClosePopUpEvent(() => { buttonsParent.SetActive(true); });

        //add pop up username exist event
        popUpUsernameExist.OpenPopUpEvent(() => { buttonsParent.SetActive(false); HideLoading(); });
        popUpUsernameExist.ClosePopUpEvent(() => { buttonsParent.SetActive(true); });

        //add pop up Room event
        popUpInRoom.OpenPopUpEvent(() => { buttonsParent.SetActive(false); HideLoading(); });
        popUpInRoom.ClosePopUpEvent(() => { buttonsParent.SetActive(true); });

        //add pop up start game event
        popUpStartFailed.OpenPopUpEvent(() => { buttonsParent.SetActive(false); HideLoading(); });
        popUpStartFailed.ClosePopUpEvent(() => { buttonsParent.SetActive(true); });
    }
    public void UsernameChangeCheck()
    {
        Debug.Log(inputUserName.text);
        UserName = inputUserName.text;
        PhotonNetwork.LocalPlayer.NickName = inputUserName.text;
    }
    public void ShowLoading()
    {
        LoadingController.Instance.ShowEff();
    }
    public void HideLoading()
    {
        LoadingController.Instance.HideEff();

    }
    public void ShowMenu()
    {
        if (!isShowMenu)
        {
            Debug.Log("show menu");
            HideLoading();
            MenuUI.ShowEffect();
            //LeanTween.moveLocalX(buttonsParent, buttonPos.transform.position.x, 0.5f).setEaseLinear();
            LeanTween.move(buttonsParent, buttonPos.transform.position, 0.5f).setEase(LeanTweenType.easeSpring);
            isShowMenu = true;
        }

    }
    public void ShowRoom()
    {

        LoadingController.Instance.loadingUI.AddShowEndEvent(() => { HideLoading(); });
        popUpInRoom.OpenPopUp();
        //popUpInRoom.OpenPopUp();
        //loadingUI.gameObject.SetActive(false);
        //loadingUI.HideEffect();
        //loadingUI.AddHideEndEvent(() => { loadingUI.HideEffect(); });
        //loadingUI.AddHideEndEvent(() => {  });
    }
    public void ShowListRoom()
    {
        popUpInRoom.ClosePopUp();
        popUpFindRoom.OpenPopUp();
        popUpCreateRoom.ClosePopUp();
        
        //loadingUI.AddHideEndEvent(() => {  });
    }
    public void ShowCreateRoomFailed()
    {
        popUpCreateRoomFailed.OpenPopUp();
    }
    public void ShowStartGameFailed()
    {
        popUpStartFailed.OpenPopUp();
    }
    public void ShowUsernameExist()
    {
        popUpUsernameExist.OpenPopUp();
    }
    public void ShowJoinRoomFailed()
    {
        popJoinRoomFailed.OpenPopUp();
    }
    public void StartGame(System.Action EndLoadEvent)
    {
        ShowLoading();
        MenuUI.gameObject.SetActive(false);
        LoadingController.Instance.loadingUI.AddShowEndEvent(() => { EndLoadEvent();});
        //LoadingController.Instance.loadingUI.AddHideEndEvent(() => { EndLoadEvent(); });
    }
    
}
