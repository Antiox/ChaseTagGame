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



        public void ResumeGame()
        {
            GameManager.ResumeGame();
        }
    }
}

