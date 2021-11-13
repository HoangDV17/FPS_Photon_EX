using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticle : MonoBehaviour
{
   private RectTransform reticle;

   [SerializeField] float restingSize, maxSize, speed;
   float currentSize;
    private void Awake()
    {
        reticle = GetComponent<RectTransform>();
    }
    private void Start()
    {
        reticle.sizeDelta = new Vector2(restingSize, restingSize);
    }
    // Update is called once per frame
    public void ReticleUpdate(float _speed)
    {
        if(_speed != 0)
        {
            currentSize = Mathf.Lerp(currentSize, maxSize, Time.deltaTime*speed);
        }
        else
        {
            currentSize = Mathf.Lerp(currentSize, restingSize, Time.deltaTime* speed);
        }
        reticle.sizeDelta = new Vector2(currentSize, currentSize);
    }
}
