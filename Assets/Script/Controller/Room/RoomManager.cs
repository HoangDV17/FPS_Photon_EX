using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instance;
    public bool isLoad = false;
    private void Awake()
    {
        if(instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
        
    }
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        //loadSceneMode = LoadSceneMode.Additive;
        if(scene.buildIndex == LevelLoader.GameScene)
        {
            //isLoad = false;
            
            CreateController.Instance.CreatePlayerManager();
            StartCoroutine(LoadScene());
        }
    }
    IEnumerator LoadScene()
    {
        isLoad = true;
        //AsyncOperation operation = SceneManager.LoadSceneAsync(index);
        float operation = PhotonNetwork.LevelLoadingProgress;
        while (operation < 1 )
        {
            float progress = Mathf.Clamp01(operation / 0.9f);
            //Debug.Log(progress);
            yield return new WaitForEndOfFrame();
        }
        isLoad = false;
    }
}
