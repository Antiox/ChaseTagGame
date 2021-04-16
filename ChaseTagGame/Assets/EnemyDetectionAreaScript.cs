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

        private void Start()
        {
            renderer = GetComponent<Renderer>();
            propertyBlock = new MaterialPropertyBlock();
        }


        private void Update()
        {
            renderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetFloat("_Cutoff", Angle);
            renderer.SetPropertyBlock(propertyBlock);
            transform.localScale = new Vector3(Range, Range, 4);
        }


        private void OnTriggerStay(Collider other)
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

                Debug.DrawRay(transform.position, direction * distance, Color.red);

                if (angle < Angle / 2f && !playerEnteredArea && playerIsVisible)
                {
                    playerEnteredArea = true;
                    playerEnterAreaCallback.Invoke(other);
                }
                else if(angle < Angle / 2f && playerIsVisible)
                    playerStayInAreaCallback.Invoke(other);
                else if (angle >= Angle / 2f || !playerIsVisible)
                {
                    playerEnteredArea = false;
                    playerLeftAreaCallback.Invoke(other);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(playerEnteredArea)
                playerLeftAreaCallback.Invoke(other);

            playerEnteredArea = false;
        }
    }
}