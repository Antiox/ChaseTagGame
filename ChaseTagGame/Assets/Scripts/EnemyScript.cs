using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    private GameObject target;
    private GameObject gameManager;

    private NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        target = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        navMeshAgent.SetDestination(target.transform.position);
        Debug.Log(navMeshAgent.isOnNavMesh);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
            gameManager.GetComponent<GameScript>().GameOver();
    }
}
