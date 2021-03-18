using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public Transform target;
    public GameObject gameManager;

    private NavMeshAgent navMeshAgent;
    private Collider proximityTriggerCollider;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        proximityTriggerCollider = GetComponentInChildren<Collider>();
    }

    void Update()
    {
        navMeshAgent.SetDestination(target.position);
        Debug.Log(navMeshAgent.isOnNavMesh);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
            gameManager.GetComponent<GameScript>().GameOver();
    }
}
