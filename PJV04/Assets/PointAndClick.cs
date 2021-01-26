using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PointAndClick : MonoBehaviour
{
    public Camera camera;

    private NavMeshAgent agent;
    private Animator anim;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        var speed = agent.velocity.magnitude;
        if (speed != 0)
        {
            anim.SetFloat("Speed", speed);
        }

        if (agent.enabled && !agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    // Reached destination, disable
                    agent.enabled = false;
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            agent.enabled = true;

            Ray mouseClickRay = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(mouseClickRay, out hit))
            {
                agent.SetDestination(hit.point);
            }

        }
    }
}
