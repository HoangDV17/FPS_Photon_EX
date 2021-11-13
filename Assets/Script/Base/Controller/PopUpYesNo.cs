
using UnityBase.Base.Controller;
using UnityBase.Base.UI;
using System;
using UnityEngine;
using UnityEngine.UI;
public class PopUpYesNo : BasePopupInScene {

    //[SerializeField]
    //protected Text txtMessage;

    [SerializeField]
    private ButtonController BtnYes;

    [SerializeField]
    private ButtonController BtnNo;
    
    //public override void ClosePopUp()
    //{
    //    ScaleTo(Vector3.zero, () => {
    //        if (_callbackClosePopUp != null)
    //            _callbackClosePopUp();
    //        if (_CloseEvent != null) _CloseEvent();
    //        gameObject.SetActive(false);
    //    });
    //}
    public void SetPopUp(Action CallBackYes =  null)
    {
        //Mes = Utils.CutString(Mes, 200);
        //txtMessage.text = Mes;
        BtnYes.OnClick((btn) =>
        {
            CallBackYes();
            ClosePopUp();
        });
        BtnNo.OnClick((btn) =>
        {
            ClosePopUp();
        });
    }
}
