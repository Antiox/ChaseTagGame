using GameLibrary;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEditor;
using System;

namespace GameLibrary
{

    public class EnemyControllerScript : MonoBehaviour
    {
        private NavMeshAgent navMeshAgent;
        private IEnemy self;
        private float searchingCooldown;
        private readonly float chasingSpeed = 7f;
        private readonly float scoutingSpeed = 5f;
        private readonly float searchingSpeed = 11f;
        private List<Vector3> searchingPath;


        void Start()
        {
            self = GameManager.WaveManager.GetEnemyFromGameObject(gameObject);
            self.Start();
            transform.position = self.GetStartingPositon();
            navMeshAgent = GetComponent<NavMeshAgent>();

            var detectionArea = transform.Find("DetectionArea").GetComponent<Renderer>();
            detectionArea.enabled = GameManager.IsOwningSkill(SkillType.DetectionArea);

            EventManager.Instance.AddListener<OnPlayerEnteredEnemyFovEvent>(OnDetectionAreaTriggerEnter);
            EventManager.Instance.AddListener<OnPlayerStayedInEnemyFovEvent>(OnDetectionAreaTriggerStay);
            EventManager.Instance.AddListener<OnPlayerLeftEnemyFovEvent>(OnDetectionAreaTriggerExit);

            StartCoroutine(Scout());
        }

        private void OnDestroy()
        {
            EventManager.Instance.RemoveListener<OnPlayerEnteredEnemyFovEvent>(OnDetectionAreaTriggerEnter);
            EventManager.Instance.RemoveListener<OnPlayerStayedInEnemyFovEvent>(OnDetectionAreaTriggerStay);
            EventManager.Instance.RemoveListener<OnPlayerLeftEnemyFovEvent>(OnDetectionAreaTriggerExit);
        }


        private IEnumerator Scout()
        {
            var propertyBlock = new MaterialPropertyBlock();
            GetComponent<Renderer>().GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor("_BaseColor", Color.blue);
            GetComponent<Renderer>().SetPropertyBlock(propertyBlock);

            navMeshAgent.speed = scoutingSpeed;

            while (self.Behavior == EnemyBehavior.Scouting)
            {
                for (int i = 0; i < self.Path.Count; i++)
                {
                    while (Vector3.Distance(transform.position, self.Path[i]) > 1.1f)
                    {
                        navMeshAgent.SetDestination(self.Path[i]);
                        yield return null;
                    }
                }
            }
        }

        private IEnumerator Chase()
        {
            var propertyBlock = new MaterialPropertyBlock();
            GetComponent<Renderer>().GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor("_BaseColor", Color.red);
            GetComponent<Renderer>().SetPropertyBlock(propertyBlock);

            navMeshAgent.speed = chasingSpeed;

            while (self.Behavior == EnemyBehavior.Chasing)
            {
                navMeshAgent.SetDestination(self.LastPlayerKnownPosition + self.LastPlayerKnownDirection * 2f);
                yield return null;
            }
        }

        private IEnumerator Search()
        {
            var propertyBlock = new MaterialPropertyBlock();
            GetComponent<Renderer>().GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor("_BaseColor", Color.yellow);
            GetComponent<Renderer>().SetPropertyBlock(propertyBlock);

            navMeshAgent.speed = searchingSpeed;
            var searchPath = Utility.GetRandomNavMeshCircularPath(self.LastPlayerKnownPosition, 6f, 2f);

            searchingPath = searchPath;

            while (self.Behavior == EnemyBehavior.Searching)
            {
                for (int i = 0; i < searchPath.Count; i++)
                {
                    do
                    {
                        navMeshAgent.SetDestination(searchPath[i]);
                        yield return null;
                    } while (navMeshAgent.GetRemainingDistance() > 0.5f);
                }
            }
        }

        private IEnumerator SearchingCooldown()
        {
            searchingCooldown = 10f;

            while (searchingCooldown > 0)
            {
                searchingCooldown -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            self.Behavior = EnemyBehavior.Scouting;
            StartCoroutine(Scout());
        }



        private Vector3 GetClosestEntity()
        {
            var players = GameObject.FindGameObjectsWithTag(GameTags.Player);
            var powerUps = GameObject.FindGameObjectsWithTag(GameTags.EnemyPowerUp);
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


        public void OnProximityTriggerEnter(Collider other)
        {
            var e = new OnEnemyTriggerEnterEvent(gameObject, other.gameObject);
            EventManager.Instance.Dispatch(e);
        }

        private void OnDetectionAreaTriggerEnter(OnPlayerEnteredEnemyFovEvent e)
        {
            if(e.Enemy == self.GameObject)
            {
                searchingCooldown = 0f;
                self.Behavior = EnemyBehavior.Chasing;
                StopAllCoroutines();
                StartCoroutine(Chase());
                self.LastPlayerKnownPosition = e.Player.transform.position;
                self.LastPlayerKnownDirection = GameManager.WaveManager.Player.Direction;
            }
        }

        private void OnDetectionAreaTriggerStay(OnPlayerStayedInEnemyFovEvent e)
        {
            if (e.Enemy == self.GameObject)
            {
                self.LastPlayerKnownPosition = e.Player.transform.position;
                self.LastPlayerKnownDirection = GameManager.WaveManager.Player.Direction;
            }
        }

        private void OnDetectionAreaTriggerExit(OnPlayerLeftEnemyFovEvent e)
        {
            if(self.Behavior == EnemyBehavior.Chasing && e.Enemy == self.GameObject)
            {
                self.Behavior = EnemyBehavior.Searching;
                StopAllCoroutines();
                StartCoroutine(Search());
                StartCoroutine(SearchingCooldown());
            }
        }
    }
}