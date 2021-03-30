using GameLibrary;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class EnemyScript : MonoBehaviour
{
    private Transform target;
    private NavMeshAgent navMeshAgent;


    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag(GameTags.Player).transform;
        StartCoroutine(DetermineTarget());
    }

    void Update()
    {
        if(target != null)
            navMeshAgent.SetDestination(target.position);
    }


    private void OnTriggerEnter(Collider other)
    {
        var e = new OnEnemyTriggerEnterEvent(gameObject, other.gameObject);
        EventManager.Instance.Broadcast(e);
    }

    private IEnumerator DetermineTarget()
    {
        while (true)
        {
            target = GetClosestTarget();
            yield return new WaitForSeconds(2f);
        }
    }

    private Transform GetClosestTarget()
    {
        var powerUps = GameObject.FindGameObjectsWithTag(GameTags.PowerUp);
        var entites = powerUps.Concat(GameObject.FindGameObjectsWithTag(GameTags.Player));

        Transform tMin = null;
        var minDist = Mathf.Infinity;
        var currentPos = transform.position;
        foreach (var t in entites)
        {
            float dist = Vector3.Distance(t.transform.position, currentPos);
            if (dist < minDist)
            {
                tMin = t.transform;
                minDist = dist;
            }
        }

        return tMin;
    }
}
