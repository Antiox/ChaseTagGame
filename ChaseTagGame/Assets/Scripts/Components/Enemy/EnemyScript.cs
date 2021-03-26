using GameLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    private GameObject target;

    private NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag(GameTags.Player);
    }

    void Update()
    {
        navMeshAgent.SetDestination(target.transform.position);
    }


    private void OnTriggerEnter(Collider other)
    {
        var e = new OnEnemyTriggerEnterEvent(gameObject, other.gameObject);
        EventManager.Instance.Broadcast(e);
    }
}
