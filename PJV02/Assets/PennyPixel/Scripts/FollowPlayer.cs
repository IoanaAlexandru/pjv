using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Vector3 offset = new Vector3(0, 0, 0);

    void Update()
    {
        transform.position = new Vector3(player.position.x + offset.x, player.position.y+offset.y, transform.position.z + offset.z);
    }
}
