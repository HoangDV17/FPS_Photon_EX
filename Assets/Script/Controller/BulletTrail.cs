using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    [SerializeField] Color color;
    [SerializeField] float speed = 10f;

    LineRenderer lr;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        color.a = Mathf.Lerp(color.a, 0, Time.deltaTime * speed);

        lr.startColor = color;
        lr.endColor = color;
    }
}
