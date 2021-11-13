using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public IEnumerator Skake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0.0f;
        
        while(elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalPos;
    }
    public IEnumerator ShootSkake(float duration, float magnitude)
    {
        //Vector3 originalPos = transform.localPosition;
        float elapsed = 0.0f;

        
        while(elapsed < duration)
        {
            float x = Random.Range(-1, 1) * magnitude;
            float y = Random.Range(0, 1) * magnitude;

            //transform.localPosition = new Vector3(x, y, originalPos.z);
            transform.parent.localEulerAngles += new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }
        //transform.localPosition = new Vector3(originalPos.x, transform.localPosition.y);
    }
}
