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


        void Start()
        {
            self = GameManager.WaveManager.GetEnemyFromGameObject(gameObject);
            self.Start();
            transform.position = self.GetStartingPositon();
            navMeshAgent = GetComponent<NavMeshAgent>();

            var detectionArea = transform.Find("DetectionArea").GetComponent<Renderer>();
            detectionArea.enabled = true; //GameManager.IsOwningSkill(SkillType.DetectionArea);

            StartCoroutine(Scout());
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(self.LastPlayerKnownPosition, 0.5f);
            Debug.DrawLine(self.LastPlayerKnownPosition, self.LastPlayerKnownDirection * 2f, Color.green, Time.deltaTime);

            if (self?.Path == null)
                return;

            for (int i = 0; i < self.Path.Count; i++)
            {
                var p = self.Path[i];
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(p, 0.5f);
                Handles.Label(p, i.ToString());
            }
        }


        private IEnumerator Scout()
        {
            while (true)
            {
                if (GameManager.State == GameState.InGame)
                {
                    switch (self.Behavior)
                    {
                        case EnemyBehavior.Scouting: ProcessScouting(); break;
                        case EnemyBehavior.Searching: ProcessSearching(); break;
                        case EnemyBehavior.Chasing: ProcessChasing(); break;
                        default: break;
                    }
                }

                yield return null;
            }
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


        private void ProcessScouting()
        {
            var propertyBlock = new MaterialPropertyBlock();
            GetComponent<Renderer>().GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor("_BaseColor", Color.blue);
            GetComponent<Renderer>().SetPropertyBlock(propertyBlock);
        }

        private void ProcessSearching()
        {
            var propertyBlock = new MaterialPropertyBlock();
            GetComponent<Renderer>().GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor("_BaseColor", Color.yellow);
            GetComponent<Renderer>().SetPropertyBlock(propertyBlock);
        }

        private void ProcessChasing()
        {
            var propertyBlock = new MaterialPropertyBlock();
            GetComponent<Renderer>().GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor("_BaseColor", Color.red);
            GetComponent<Renderer>().SetPropertyBlock(propertyBlock);
        }



        public void OnProximityTriggerEnter(Collider other)
        {
            var e = new OnEnemyTriggerEnterEvent(gameObject, other.gameObject);
            EventManager.Instance.Dispatch(e);
        }

        public void OnDetectionAreaTriggerEnter(Collider other)
        {
            StopCoroutine(SearchingCooldown());
            self.Behavior = EnemyBehavior.Chasing;
            self.LastPlayerKnownPosition = other.transform.position;
            self.LastPlayerKnownDirection = GameManager.WaveManager.Player.Direction;
        }

        public void OnDetectionAreaTriggerStay(Collider other)
        {
            self.Behavior = EnemyBehavior.Chasing;
            self.LastPlayerKnownPosition = other.transform.position;
            self.LastPlayerKnownDirection = GameManager.WaveManager.Player.Direction;
        }

        public void OnDetectionAreaTriggerExit(Collider other)
        {
            if(self.Behavior == EnemyBehavior.Chasing)
            {
                self.Behavior = EnemyBehavior.Searching;
                StartCoroutine(SearchingCooldown());
            }
        }

        private IEnumerator SearchingCooldown()
        {
            yield return new WaitForSeconds(10f);
            self.Behavior = EnemyBehavior.Scouting;
        }
    }
}