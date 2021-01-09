using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    float scale = 1f;
    [SerializeField] bool growing = true;
    [SerializeField] int scaleSpeed = 5;

    void Update()
    {
        if (growing)
        {
            scale += 0.01f * scaleSpeed * Time.deltaTime;
        } else
        {
            scale -= 0.01f * scaleSpeed * Time.deltaTime;
        }

        if (scale < 0.95 || scale > 1.05)
        {
            growing = !growing;
        }

        transform.localScale = new Vector2(scale, scale);
    }
}
