using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace GameLibrary
{
    public class EnemyDetectionAreaScript : MonoBehaviour
    {
        public float Range { get; set; }
        public float Angle { get; set; }

        [SerializeField] private UnityEvent<Collider> playerEnterAreaCallback;
        [SerializeField] private UnityEvent<Collider> playerStayInAreaCallback;
        [SerializeField] private UnityEvent<Collider> playerLeftAreaCallback;
        [SerializeField] private LayerMask targetMask;
        private bool playerEnteredArea;

        private new Renderer renderer;
        private MaterialPropertyBlock propertyBlock;



        void Start()
        {
            renderer = GetComponent<Renderer>();
            propertyBlock = new MaterialPropertyBlock();
        }

        void Update()
        {
            renderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetFloat("_Cutoff", Angle);
            renderer.SetPropertyBlock(propertyBlock);
            transform.localScale = new Vector3(Range, Range, 4);
        }

        void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(GameTags.Player))
            {
                var otherPosition = other.transform.position + Vector3.up;
                var direction = (otherPosition - transform.position).normalized;
                var planeDirection = otherPosition - transform.position;
                planeDirection.y = 0;
                planeDirection = planeDirection.normalized;

                var angle = Vector3.Angle(transform.parent.forward, planeDirection);
                var distance = Vector3.Distance(otherPosition, transform.position);
                var playerIsVisible = !Physics.Raycast(transform.position, direction, distance, targetMask, QueryTriggerInteraction.Ignore);


                if ((distance < 3f || (angle < Angle / 2f && playerIsVisible)) && !playerEnteredArea)
                {
                    playerEnteredArea = true;
                    InvokePlayerEnteredFovEvent(other);
                }
                else if ((angle < Angle / 2f && playerIsVisible) || distance < 3f)
                    InvokePlayerStayedFovEvent(other);
                else if (angle >= Angle / 2f || !playerIsVisible)
                {
                    playerEnteredArea = false;
                    InvokePlayerLeftFovEvent(other);
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            if(playerEnteredArea)
                InvokePlayerLeftFovEvent(other);

            playerEnteredArea = false;
        }




        private void InvokePlayerEnteredFovEvent(Collider player)
        {
            var e = new OnPlayerEnteredEnemyFovEvent(player, transform.parent.gameObject);
            EventManager.Instance.Dispatch(e);
        }

        private void InvokePlayerStayedFovEvent(Collider player)
        {
            var e = new OnPlayerStayedInEnemyFovEvent(player, transform.parent.gameObject);
            EventManager.Instance.Dispatch(e);
        }

        private void InvokePlayerLeftFovEvent(Collider player)
        {
            var e = new OnPlayerLeftEnemyFovEvent(player, transform.parent.gameObject);
            EventManager.Instance.Dispatch(e);
        }
    }
}