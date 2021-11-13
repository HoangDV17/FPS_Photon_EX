using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    // Start is called before the first frame update
    public int speed;

    protected virtual void Move(Vector3 direction)
    {
        LeanTween.moveLocal(gameObject, this.transform.position + direction * speed, 0.1f);
    }
    // Update is called once per frame
}
