using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameLibrary
{
    public class MenuScript : MonoBehaviour
    {
        void Awake()
        {
            EventManager.Instance.AddListener<OnGameOverEvent>(OnGameOver);
        }

        void OnDestroy()
        {
            EventManager.Instance.RemoveListener<OnGameOverEvent>(OnGameOver);
        }

        public void StartGame()
        {
            GameManager.Reset();
            SceneManager.LoadScene(1);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void GoToMainMenu()
        {
            SceneManager.LoadScene(0);
        }

        public void MouseSensitivitySliderValueChanged(Slider slider)
        {
            var e = new OnMouseSensitivityChangedEvent(slider.value);
            EventManager.Instance.Dispatch(e);
        }

        public void ResumeGame()
        {
            GameManager.ResumeGame();
        }



        private void OnGameOver(OnGameOverEvent e)
        {
            var scoreLabel = GameObject.Find("ScoreLabel").GetComponent<TextMeshProUGUI>();
            scoreLabel.text = $"Day {e.Day.Number}";
        }
    }
}

