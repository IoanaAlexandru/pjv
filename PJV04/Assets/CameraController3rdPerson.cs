using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController3rdPerson : MonoBehaviour
{
    Transform player;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        transform.position = player.position + offset;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + offset;
        transform.LookAt(player.position);
    }
}
