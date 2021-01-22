using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalCameraController : MonoBehaviour
{
    public Camera cam1;
    public Camera cam3;

    void Start()
    {
        cam1.enabled = false;
        cam3.enabled = true;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.C))
        {
            cam1.enabled = !cam1.enabled;
            cam3.enabled = !cam3.enabled;
        }
    }
}
