using UnityEngine;
using UnityEngine.AI;

public class ZombiePathfinding : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.position);
    }
}
