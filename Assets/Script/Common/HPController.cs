using System.Collections;
using System.Collections.Generic;
using UnityBase.Base.Controller;
using UnityEngine;
using UnityEngine.UI;

public class HPController : ProcessController
{
    public delegate void Die();
    public Die dieEvent;
    public Transform ui;
    public Image healthBar;
    public Image realHP;
   
    public int max
    {
        get
        {
            return (int)maxValue;
        }
    }
    public float HP
    {
        get
        {
            return currentsValue;
        }
    }

    protected virtual void Start()
    {
        //healthBar = ui.GetChild(0).GetComponent<Image>();
        //realHP = healthBar.transform.GetChild(0).GetComponent<Image>();
        currentsValue = maxValue;
    }
    public void SetMaxHp(int value)
    {
        maxValue += value;
        if(currentsValue > maxValue)
        {
            EditValue(maxValue);
        }
        else
        {
            EditValue(currentsValue += value);
        }
    }
    public void SetHP(int newHP)
    {
        maxValue = newHP;
        EditValue(currentsValue = newHP);
    }
    public void SetCurrentHP(float ChangeValue)
    {
        EditValue( ChangeValue);
    }
    public void ChangeHP(float changeHP)
    {
        EditValue(currentsValue - changeHP);
    }

    protected override void OnUpdate(float value)
    {
        realHP.fillAmount = value / maxValue;
        if (value <= 0.1f)
        {
            if (dieEvent != null)
            {
                dieEvent();
            }
        }
    }
    private void Update()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, currentsValue / maxValue, 0.05f);
    }
}
