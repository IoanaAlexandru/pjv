using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxController : MonoBehaviour
{
    public float playerFollowDistance = 10;
    public float playerAttackDistance = 2;
    public float speed = 10;

    GameObject player;
    Animator anim;
    Rigidbody rigidbody;

    bool attacking;
    bool running;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var playerDistance = Vector3.Distance(player.transform.position, transform.position);
        if (playerDistance <= playerFollowDistance && !attacking)
        {
            transform.LookAt(player.transform);
            if (!running)
            {
                running = true;
                anim.Play("Fox_Run_InPlace");
            }
            rigidbody.velocity = transform.forward * speed;
        }
        if (playerDistance <= playerAttackDistance)
        {
            if (!attacking)
            {
                attacking = true;
                anim.SetBool("canAttack", true);
                anim.Play("Fox_Attack_Paws");
            }
        }
        else
        {
            attacking = false;
            anim.SetBool("canAttack", false);
        }

    }
}
