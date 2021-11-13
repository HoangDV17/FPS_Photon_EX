using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityBase.Base.UI;
using UnityBase.Base.UI.Effect;

public class TableCountSlot : MonoBehaviour
{
    [SerializeField] GameObject pf;
    [SerializeField] Transform parrent, canvas;
    [SerializeField] Text content;
    EffectSlide effectSlike;
    CanvasGroup alpha;
    private void Start()
    {
        effectSlike = GetComponent<EffectSlide>();
        alpha = GetComponent<CanvasGroup>();
        //effectSlike.AddShowEndEvent(()=> {StartCoroutine(TimeLife(5f));});
        effectSlike.ShowEffect();
    }
    public IEnumerator TimeLife(float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            alpha.alpha = 1 - elapsed/duration;
            yield return null;
        }
        Destroy(gameObject);
    }
    public void SetContent(string Killer, KillType killType, string playerSlain)
    {
        //KillType _type = killType;
        switch(killType)
        {
            case KillType.normal:
            content.text = Killer + " " + " <color=red><size=48>KILL</size></color> " + " " + playerSlain; 
            break;
            case KillType.bomb:
            content.text = Killer + " " + " <color=red><size=48>KILL</size></color> " + " " + playerSlain; 
            break;
            case KillType.headshot:
            content.text = Killer + " " + " <color=red><size=48>HEAD SHOT</size></color> " + " " + playerSlain; 
            break;
            case KillType.knife:
            content.text = Killer + " " + " <color=red><size=48>KILL</size></color> " + " " + playerSlain; 
            break;
            case KillType.suicide:
            content.text = Killer + " " + " <color=red><size=48>DIE BY BOMB</size></color> " + " " + playerSlain; 
            break;
        }
        
    }
    // Update is called once per frame
    
}
public enum KillType{normal, headshot, bomb, knife, suicide}
