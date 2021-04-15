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

        [SerializeField] private UnityEvent<Collider> playerDetectedCallback;
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
                var direction = (other.transform.position - transform.position).normalized;
                var angle = Vector3.Angle(transform.parent.forward, direction);
                if (angle < Angle / 2f && !playerEnteredArea)
                {
                    playerEnteredArea = true;
                    playerDetectedCallback.Invoke(other);
                }
                else if (angle >= Angle / 2f)
                    playerEnteredArea = false;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            playerEnteredArea = false;
        }
    }
}