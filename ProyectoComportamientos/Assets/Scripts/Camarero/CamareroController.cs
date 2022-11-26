using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CamareroController : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

    }
    void Update()
    {
        //Vector3 destination = new Vector3(5, 0, 5);
        //navMeshAgent.destination = destination;
    }
}
