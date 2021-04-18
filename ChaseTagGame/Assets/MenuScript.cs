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
        public void StartGame()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            GameManager.Reset();
            SceneManager.LoadScene(1);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void GoToMainMenu()
        {
            GameManager.Reset();
            SceneManager.LoadScene(0);

            var e = new OnGameQuittedEvent();
            EventManager.Instance.Dispatch(e);
        }

        public void ResumeGame()
        {
            GameManager.ResumeGame();
        }

        public void GoToNextDay()
        {
            GameManager.GoToNextDay();
        }
    }
}

