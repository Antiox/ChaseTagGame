using System;
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
        [SerializeField] private GameObject shopMenu;
        private TextMeshProUGUI dayInfoLabel;
        private TextMeshProUGUI dayEndedLabel;
        private TextMeshProUGUI fpsCounterLabel;
        private TextMeshProUGUI objectivesCounterLabel;
        private TextMeshProUGUI gemsCounterLabel;
        private TextMeshProUGUI currencyAmountLabel;
        private TextMeshProUGUI scoreLabel;
        private TextMeshProUGUI interactiveElementLabel;



        public void Start()
        {
            dayInfoLabel = GameObject.Find("DayInfoLabel").GetComponent<TextMeshProUGUI>();
            dayEndedLabel = GameObject.Find("DayEndedLabel").GetComponent<TextMeshProUGUI>();
            fpsCounterLabel = GameObject.Find("FpsCounterLabel").GetComponent<TextMeshProUGUI>();
            objectivesCounterLabel = GameObject.Find("ObjectivesCounterLabel").GetComponent<TextMeshProUGUI>();
            gemsCounterLabel = GameObject.Find("GemsCounterLabel").GetComponent<TextMeshProUGUI>();
            interactiveElementLabel = GameObject.Find("InteractiveElementLabel").GetComponent<TextMeshProUGUI>();
            scoreLabel = GameObject.Find("ScoreLabel").GetComponent<TextMeshProUGUI>();
            optionsMenu.SetActive(false);
            gameOverMenu.SetActive(false);
            shopMenu.SetActive(false);
            dayEndedLabel.enabled = false;
            interactiveElementLabel.enabled = false;


            StartCoroutine(UpdateFps());

            EventManager.Instance.AddListener<OnSkillBoughtEvent>(OnSkillBought);
        }

        public void OnDestroy()
        {
            EventManager.Instance.RemoveListener<OnSkillBoughtEvent>(OnSkillBought);
        }



        public void DisplayDayInfo(DayInfo day)
        {
            dayInfoLabel.text = day.ToString();
            objectivesCounterLabel.text = $"{day.ObjectsCollected}/{day.RequiredObjects} Keys";
            gemsCounterLabel.text = $"{day.GemsCollected} Gems";
        }

        public void ResumeGame()
        {
            optionsMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void PauseGame()
        {
            optionsMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void DisplayGameOver(DayInfo day)
        {
            gameOverMenu.SetActive(true);
            scoreLabel.text = $"Day {day.Number}";
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void DisplayEndOfDay()
        {
            dayEndedLabel.enabled = true;
        }

        public void DisplayShopMenu()
        {
            shopMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void DisplayCurrencyAmount(int amount)
        {
            currencyAmountLabel = shopMenu.transform.Find("CurrencyCountLabel").GetComponent<TextMeshProUGUI>();
            currencyAmountLabel.text = $"Currency : {amount}";
        }

        public void DisplaySkills(List<Skill> skills)
        {
            var skillsContainer = shopMenu.transform.Find("SkillsContainer");

            foreach (var s in skills)
            {
                var skillGameObject = Instantiate(Resources.Load<GameObject>("Prefabs/SkillPanel"), skillsContainer.transform);
                skillGameObject.GetComponent<SkillRendererScript>().Skill = s;
            }
        }

        private IEnumerator UpdateFps()
        {
            while (true)
            {
                fpsCounterLabel.text = (1f / Time.unscaledDeltaTime).ToString("N0") + " FPS";
                yield return new WaitForSeconds(0.5f);
            }
        }

        private void OnSkillBought(OnSkillBoughtEvent e)
        {
            Destroy(e.SkillPanel);
            DisplayCurrencyAmount(e.NewCurrency);
        }
    }
}

