using System.Collections;
using System.Collections.Generic;
using UnityBase.Base.UI.Effect;
using UnityEngine;

public class LoadingPopUp : MonoBehaviour
{
    [SerializeField] List<EffectAlphaShow> effectAlphas;

    public void ShowEff()
    {
        effectAlphas.ForEach(eff =>
        {
            eff.ShowEffect();
        });
    }
    public void HideEff()
    {
        effectAlphas.ForEach(eff =>
        {
            eff.HideEffect();
        });
    }
}
