using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLibrary
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private PlayerInputs inputs;

        public Transform CameraTransform { get; set; }
        public Transform CameraTargetTransform { get; set; }

        private CinemachineFreeLook freeLookCamera;

        void Start()
        {
            freeLookCamera = GetComponentInChildren<CinemachineFreeLook>();
        }

        private void Update()
        {
            CameraTransform = freeLookCamera.transform;
            CameraTargetTransform = freeLookCamera.LookAt.transform;
        }
    }
}
