using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class ButterChickenAI : MonoBehaviour
{
    public float wanderRange, stillRange, fleeRange;
    public float wanderSpeed, fleeSpeed, baitSpeed;
    public NavMeshAgent navMeshAgent;
    public Vector3 startpos;
    public GameObject player;
    public int state;
    public Animator animator;

    public AudioSource passiveSource, scareSource;

    public static event Action<string> chickenEncounter;
    // Update is called once per frame

    private void Start()
    {
        startpos = transform.position;
        player = GameObject.FindAnyObjectByType<FirstPersonControls>().gameObject;
    }
    void Update()
    {
        if(navMeshAgent.velocity.magnitude>0.25) {
            animator.Play("Walk");
        }
        else
        {
            animator.Play("New State");
        }
        float playerDistance = Vector3.Distance(player.transform.position, transform.position);
        bool playerIsMoving = player.GetComponent<CharacterController>().velocity.magnitude > 0;

        if (state == 0)
        {
            if (playerDistance <= stillRange && !playerIsMoving)
            {
                StartCoroutine(waitStill());
            }
            
        }
        if (playerDistance <= fleeRange && playerIsMoving)
        {
            state = 1;
            if (!scareSource.isPlaying) scareSource.Play();
            if(player.GetComponent<FirstPersonControls>().holdingOscie && player.GetComponent<FirstPersonControls>().uncompletedTasks.Contains("ChickenEncounterFirstTime"))
            {
                player.GetComponent<FirstPersonControls>().completedTasks.Add("ChickenEncounterFirstTime");
                player.GetComponent<FirstPersonControls>().uncompletedTasks.Remove("ChickenEncounterFirstTime");
                chickenEncounter("ChickenEncounterFirstTime");
            }
            
        }
        else if (playerDistance > stillRange)
        {
            navMeshAgent.speed = wanderSpeed;
            state = 0;
            if(!passiveSource.isPlaying) passiveSource.Play();

        }

        switch (state)
        {
            case 0:
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.magnitude == 0f)
                {
                    navMeshAgent.SetDestination(startpos + UnityEngine.Random.insideUnitSphere * wanderRange);
                }
                animator.speed = 0.75f;
                break;
            case 1:
                Vector3 fleeDirection = (transform.position - player.transform.position).normalized;
                navMeshAgent.speed = fleeSpeed;
                navMeshAgent.SetDestination(transform.position + fleeDirection * fleeRange);
                animator.speed = 1.25f;
                break;
                
            case 2:
                navMeshAgent.speed = baitSpeed;
                Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
                Vector3 stoppingPoint = player.transform.position - directionToPlayer * 3.5f; 
                navMeshAgent.SetDestination(stoppingPoint);
                animator.speed = 0.5f;
                break;
        }
      

    
    }
    public IEnumerator waitStill()
    {
        yield return new WaitForSeconds(3f);
        if ((Vector3.Distance(player.transform.position, transform.position) <= stillRange) && player.GetComponent<CharacterController>().velocity.magnitude ==0)
        {
            state = 2;
        }
    }
   
}
