using Cinemachine;
using System;
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
            EventManager.Instance.AddListener<OnGameOverEvent>(OnGameOver);
            EventManager.Instance.AddListener<OnMouseSensitivityChangedEvent>(OnMouseSensitivityChanged);
        }

        void Update()
        {
            CameraTransform = freeLookCamera.transform;
            CameraTargetTransform = freeLookCamera.LookAt.transform;
        }

        void OnDestroy()
        {
            EventManager.Instance.RemoveListener<OnGameOverEvent>(OnGameOver);
            EventManager.Instance.RemoveListener<OnMouseSensitivityChangedEvent>(OnMouseSensitivityChanged);
        }


        private void OnGameOver(OnGameOverEvent e)
        {
            freeLookCamera.m_XAxis.m_InputAxisName = "";
            freeLookCamera.m_YAxis.m_InputAxisName = "";
            freeLookCamera.m_YAxis.m_MaxSpeed = 0;
            freeLookCamera.m_XAxis.m_MaxSpeed = 0;
        }

        private void OnMouseSensitivityChanged(OnMouseSensitivityChangedEvent e)
        {
            freeLookCamera.m_XAxis.m_MaxSpeed = 60f * e.Sensitivity;
            freeLookCamera.m_YAxis.m_MaxSpeed = 0.42f * e.Sensitivity;
        }
    }
}
