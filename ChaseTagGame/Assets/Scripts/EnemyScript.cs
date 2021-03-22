using GameLibrary;
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
        gameManager = GameObject.FindGameObjectWithTag(GameTags.GameManager);
        target = GameObject.FindGameObjectWithTag(GameTags.Player);
    }

    void Update()
    {
        navMeshAgent.SetDestination(target.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        gameManager.GetComponent<GameScript>().NotifyEnemyTriggerEnter(other, gameObject);
    }
}
