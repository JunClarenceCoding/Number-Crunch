using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCNavigation : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject Goal;
    private bool shouldFollow = false;
    public GameObject secondTriggerBox; 
    [SerializeField] private Animator animator;
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component is missing.");
        }
        animator = this.GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component is missing.");
        }
        Goal = GameObject.Find("Goal");
        if (Goal == null)
        {
            Debug.LogError("Goal GameObject not found.");
        }
    }
    void Update()
    {
        if (shouldFollow && agent != null && Goal != null)
        {
            agent.speed = 10;
            agent.SetDestination(Goal.transform.position);
        }
        else if (agent != null)
        {
            agent.speed = 0;
        }
        if (shouldFollow && agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            OnDestinationReached();
        }
    }
    public void OnPressedFollow()
    {
        shouldFollow = true;

        if (animator != null)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            Debug.LogError("Animator component is null");
        }
    }
    private void OnDestinationReached()
    {
        shouldFollow = false; 
        if (animator != null)
        {
            animator.SetBool("isRunning", false);
        }
        if (secondTriggerBox != null)
        {
            secondTriggerBox.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Second trigger box reference is missing.");
        }
    }
}