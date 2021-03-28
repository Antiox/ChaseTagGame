using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GameLibrary
{
    public class HudManagerScript : MonoBehaviour
    {
        private TextMeshProUGUI gamePausedLabel;
        private TextMeshProUGUI gameOverLabel;
        private TextMeshProUGUI dayInfoLabel;
        private TextMeshProUGUI dayEndedLabel;
        private TextMeshProUGUI fpsCounterLabel;

        public void Start()
        {
            gamePausedLabel = GameObject.Find("PausedLabel").GetComponent<TextMeshProUGUI>();
            gameOverLabel = GameObject.Find("GameOverLabel").GetComponent<TextMeshProUGUI>();
            dayInfoLabel = GameObject.Find("DayInfoLabel").GetComponent<TextMeshProUGUI>();
            dayEndedLabel = GameObject.Find("DayEndedLabel").GetComponent<TextMeshProUGUI>();
            fpsCounterLabel = GameObject.Find("FpsCounterLabel").GetComponent<TextMeshProUGUI>();

            gamePausedLabel.enabled = false;
            gameOverLabel.enabled = false;
            dayEndedLabel.enabled = false;


            StartCoroutine(UpdateFps());
        }



        public void DisplayDayInfo(DayInfo day)
        {
            dayInfoLabel.text = day.ToString();
        }

        public void ResumeGame()
        {
            gamePausedLabel.enabled = false;
        }

        public void PauseGame()
        {
            gamePausedLabel.enabled = true;
        }

        public void DisplayGameOver()
        {
            gameOverLabel.enabled = true;
        }

        public void DisplayEndOfDay()
        {
            dayEndedLabel.enabled = true;
        }

        private IEnumerator UpdateFps()
        {
            while (true)
            {
                fpsCounterLabel.text = (1f / Time.unscaledDeltaTime).ToString("N0") + " FPS";
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}

