using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GameLibrary
{
    public class HudManagerScript : MonoBehaviour
    {
        [SerializeField] private GameObject optionsMenu;
        [SerializeField] private GameObject gameOverMenu;
        private TextMeshProUGUI dayInfoLabel;
        private TextMeshProUGUI dayEndedLabel;
        private TextMeshProUGUI fpsCounterLabel;
        private TextMeshProUGUI objectivesCounterLabel;

        public void Start()
        {
            dayInfoLabel = GameObject.Find("DayInfoLabel").GetComponent<TextMeshProUGUI>();
            dayEndedLabel = GameObject.Find("DayEndedLabel").GetComponent<TextMeshProUGUI>();
            fpsCounterLabel = GameObject.Find("FpsCounterLabel").GetComponent<TextMeshProUGUI>();
            objectivesCounterLabel = GameObject.Find("ObjectivesCounterLabel").GetComponent<TextMeshProUGUI>();

            optionsMenu.SetActive(false);
            gameOverMenu.SetActive(false);
            dayEndedLabel.enabled = false;


            StartCoroutine(UpdateFps());
        }



        public void DisplayDayInfo(DayInfo day)
        {
            dayInfoLabel.text = day.ToString();
            objectivesCounterLabel.text = $"{day.ObjectsCollected}/{day.RequiredObjects}";
        }

        public void ResumeGame()
        {
            optionsMenu.SetActive(false);
        }

        public void PauseGame()
        {
            optionsMenu.SetActive(true);
        }

        public void DisplayGameOver()
        {
            gameOverMenu.SetActive(true);
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

