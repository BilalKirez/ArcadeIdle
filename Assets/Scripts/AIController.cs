using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public Transform itemToPickup;
    public Transform itemDropLocation;
    private NavMeshAgent agent;
    public bool isDroping = false;
    [SerializeField] private ObjectCollectionInteraction objectCollection;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        objectCollection = GetComponent<ObjectCollectionInteraction>();
        agent = GetComponent<NavMeshAgent>();
        InvokeRepeating("AIWorking", 0f, 0.02f);
    }
    // Update is called once per frame
    void Update()
    {
        if (objectCollection.spawnerMachine.spawnObjects.Count > 0 && objectCollection.transformerMachine.EnoughSpaceForStorage())
        {

        }
        if (objectCollection.itemList.Count == 0)
        {
            isDroping = false;
        }
        if (agent.remainingDistance < 0.1f)
        {
            animator.SetBool("run", false);
        }
        else
        {
            animator.SetBool("run", true);
        }
    }

    void GoToDestination(Transform destination)
    {
        agent.SetDestination(destination.position);
    }
    public void AIWorking()
    {
        if (objectCollection.itemList.Count < objectCollection.carryItemCapacity && !isDroping)
        {
            GoToDestination(itemToPickup);
        }
        else
        {
            isDroping = true;
            GoToDestination(itemDropLocation);
        }
    }
}
