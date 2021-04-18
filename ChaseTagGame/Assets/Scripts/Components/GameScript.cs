using GameLibrary;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Collections;
using UnityEngine.AI;
using UnityEditor;
using System.Collections.Generic;

namespace GameLibrary
{

    public class GameScript : MonoBehaviour
    {
        void Start()
        {
            Time.timeScale = 1f;
            GameManager.Start();
            StartCoroutine(SpawnPowerUp(15f));
        }

        void Update()
        {
            GameManager.Update();
        }

        void OnDestroy()
        {
            GameManager.OnDestroy();
            StopCoroutine(SpawnPowerUp(15f));
        }


        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void GoToMainMenu()
        {
            SceneManager.LoadScene(0);
        }



        private static IEnumerator SpawnPowerUp(float delay)
        {
            while (true)
            {
                var e = new OnPowerUpSpawnRequestedEvent();
                EventManager.Instance.Dispatch(e);
                yield return new WaitForSeconds(delay);

                if (GameManager.State == GameState.Shopping || GameManager.State == GameState.GameOver)
                    break;
            }
        }
    }
}
