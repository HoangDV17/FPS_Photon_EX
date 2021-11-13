using System.Collections;
using System.Collections.Generic;
using UnityBase.Base.UI.Effect;
using UnityEngine;

public class LoadingController : SingletonMonoBehavier<LoadingController>
{
    public EffectAlphaShow loadingUI;
    private void Awake()
    {
        //DontDestroyOnLoad(this);
        loadingUI = GetComponent<EffectAlphaShow>();
    }
    public void ShowEff()
    {
        loadingUI.ShowEffect();
    }
    public void HideEff()
    {
        loadingUI.HideEffect();
    }
}
