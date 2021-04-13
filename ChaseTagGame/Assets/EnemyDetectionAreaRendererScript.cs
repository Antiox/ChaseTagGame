using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLibrary
{
    public class EnemyDetectionAreaRendererScript : MonoBehaviour
    {
        private MaterialPropertyBlock _propBlock;
        private Renderer _renderer;
        private float _angle;


        private void Awake()
        {
            _propBlock = new MaterialPropertyBlock();
            _renderer = GetComponent<Renderer>();
            _angle = Random.Range(0f, 360f);
        }

        void Update()
        {
            _renderer.GetPropertyBlock(_propBlock);
            _propBlock.SetFloat("_Cutoff", _angle);
            _renderer.SetPropertyBlock(_propBlock);
        }
    }
}