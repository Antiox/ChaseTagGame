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

        void Awake()
        {
            EventManager.Instance.AddListener<OnGameOverEvent>(OnGameOver);
        }

        void Start()
        {
            InitializeValues();
        }

        void OnDestroy()
        {
            EventManager.Instance.RemoveListener<OnGameOverEvent>(OnGameOver);
        }


        public void MouseSensitivitySliderValueChanged(Slider slider)
        {
            var e = new OnSettingChangedEvent(Settings.MouseSensitivity, slider.value);
            EventManager.Instance.Dispatch(e);
        }


        private void OnGameOver(OnGameOverEvent e)
        {
            var scoreLabel = GameObject.Find("ScoreLabel").GetComponent<TextMeshProUGUI>();
            scoreLabel.text = $"Day {e.Day.Number}";
        }


        private void InitializeValues()
        {
            if(mouseSensitivitySlider !=  null && PlayerPrefs.HasKey(Settings.MouseSensitivity.ToString("g")))
                mouseSensitivitySlider.value = PlayerPrefs.GetFloat(Settings.MouseSensitivity.ToString("g"));
        }
    }
}