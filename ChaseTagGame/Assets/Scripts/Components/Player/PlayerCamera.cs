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
            EventManager.Instance.AddListener<OnSettingChangedEvent>(OnMouseSensitivityChanged);
            EventManager.Instance.AddListener<OnDayEndedEvent>(OnDayEnded);
            ChangeCameraSensitivity(SettingsManager.Instance.GetSettingValue(SettingType.MouseSensitivity));
        }

        void Update()
        {
            CameraTransform = freeLookCamera.transform;
            CameraTargetTransform = freeLookCamera.LookAt.transform;
        }

        void OnDestroy()
        {
            EventManager.Instance.RemoveListener<OnGameOverEvent>(OnGameOver);
            EventManager.Instance.RemoveListener<OnDayEndedEvent>(OnDayEnded);
            EventManager.Instance.RemoveListener<OnSettingChangedEvent>(OnMouseSensitivityChanged);

        }



        private void FreezeCamera()
        {
            freeLookCamera.m_XAxis.m_InputAxisName = "";
            freeLookCamera.m_YAxis.m_InputAxisName = "";
            freeLookCamera.m_YAxis.m_MaxSpeed = 0;
            freeLookCamera.m_XAxis.m_MaxSpeed = 0;
        }

        private void OnGameOver(OnGameOverEvent e)
        {
            FreezeCamera();
        }

        private void OnDayEnded(OnDayEndedEvent e)
        {
            FreezeCamera();
        }

        private void ChangeCameraSensitivity(float value)
        {
            freeLookCamera.m_XAxis.m_MaxSpeed = 60f * value;
            freeLookCamera.m_YAxis.m_MaxSpeed = 0.42f * value;
        }



        private void OnMouseSensitivityChanged(OnSettingChangedEvent e)
        {
            if(e.Setting == SettingType.MouseSensitivity)
                ChangeCameraSensitivity(e.Value);
        }
    }
}
