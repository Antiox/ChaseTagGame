using GameLibrary;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEditor;

public class EnemyScript : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private List<Vector3> path;


    void Start()
    {
        path = Utility.GetRandomNavMeshPath();
        transform.position = path[0];
        navMeshAgent = GetComponent<NavMeshAgent>();
        StartCoroutine(FollowPath());
    }

    private void OnTriggerEnter(Collider other)
    {
        var e = new OnEnemyTriggerEnterEvent(gameObject, other.gameObject);
        EventManager.Instance.Dispatch(e);
    }

    private IEnumerator FollowPath()
    {
        Vector3 closestWaypoint;
        while (true)
        {
            if(GameManager.State == GameState.InGame)
            {
                for (int i = 0; i < path.Count; i++)
                {
                    do
                    {
                        var closestEntity = GetClosestTarget();
                        var distanceToNextPathPoint = (transform.position - path[i]).sqrMagnitude;
                        var distanceToClosestEntity = (transform.position - closestEntity).sqrMagnitude;
                        var minDistance = Mathf.Min(distanceToNextPathPoint, distanceToClosestEntity);
                        closestWaypoint = Utility.ApproximatelyEquals(distanceToNextPathPoint, minDistance) ? path[i] : closestEntity;
                        navMeshAgent.SetDestination(closestWaypoint);
                        yield return null;
                    } while ((transform.position - closestWaypoint).sqrMagnitude > 1.1f * 1.1f);
                }
            }

            yield return null;
        }
    }

    private Vector3 GetClosestTarget()
    {
        var players = GameObject.FindGameObjectsWithTag(GameTags.Player);
        var powerUps = GameObject.FindGameObjectsWithTag(GameTags.PowerUp)?.Where(p => p.GetComponent<PowerUpScript>()?.faction == PowerUpFaction.Enemy);
        var entites = players.Concat(powerUps);

        var tMin = Vector3.positiveInfinity;
        var minDist = Mathf.Infinity;
        var currentPos = transform.position;
        foreach (var t in entites)
        {
            float dist = Vector3.Distance(t.transform.position, currentPos);
            if (dist < minDist)
            {
                tMin = t.transform.position;
                minDist = dist;
            }
        }

        return tMin;
    }
}
