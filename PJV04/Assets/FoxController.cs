using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoxController : MonoBehaviour
{
    public float playerFollowDistance = 10;
    public float playerAttackDistance = 2;
    public float speed = 10;

    public int health = 50;
    public Slider healthBar;

    GameObject player;
    Animator anim;
    Rigidbody rigidbody;

    bool attacking;
    bool running;
    bool dead;

    // Start is called before the first frame update
    void Start()
    {
        healthBar.enabled = false;
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dead)
        {
            return;
        }

        healthBar.value = health;
        if (health <= 0 && !dead)
        {
            dead = true;
            anim.Play("Fox_Falling_Left");
        }

        var playerDistance = Vector3.Distance(player.transform.position, transform.position);
        if (playerDistance <= playerFollowDistance && !attacking)
        {
            healthBar.enabled = true;
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
