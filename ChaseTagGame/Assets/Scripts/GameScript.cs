using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScript : MonoBehaviour
{
    public Text gamePausedLabel;
    public Text gameOverLabel;
    public Text pointCounterLabel;


    private bool isPaused = false;
    private int points = 0;

    public void Start()
    {
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            if (isPaused)
                PauseGame();
            else
                UnpauseGame();
        }

        pointCounterLabel.text = points.ToString("N0");
    }

    public void GameOver()
    {
        gameOverLabel.enabled = true;
        Time.timeScale = 0.01f;
        Invoke("RestartGame", 3f * Time.timeScale);
    }

    public void PauseGame()
    {
        gamePausedLabel.enabled = true;
        Time.timeScale = 0;
    }

    public void UnpauseGame()
    {
        gamePausedLabel.enabled = false;
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddPoints(int p)
    {
        points += p;
    }
}
