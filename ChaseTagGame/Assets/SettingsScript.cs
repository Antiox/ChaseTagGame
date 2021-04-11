using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameLibrary
{
    public class SettingsScript : MonoBehaviour
    {
        [SerializeField] private Slider mouseSensitivitySlider;

        void Start()
        {
            InitializeValues();
        }

        public void MouseSensitivitySliderValueChanged(Slider slider)
        {
            var e = new OnSettingChangedEvent(SettingType.MouseSensitivity, slider.value);
            EventManager.Instance.Dispatch(e);
        }

        private void InitializeValues()
        {
            mouseSensitivitySlider.value = SettingsManager.Instance.GetSettingValue(SettingType.MouseSensitivity);
        }
    }
}