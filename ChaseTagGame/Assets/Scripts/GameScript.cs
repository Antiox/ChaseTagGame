using GameLibrary;
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
    public Text fpsCounterLabel;

    private GameObject player;
    private PowerUpManager powerUpManager;

    private bool isPaused = false;
    private float points = 0;
    private float fpsSmoothing = 0;

    public void Start()
    {
        Time.timeScale = 1f;

        powerUpManager = new PowerUpManager();
        player = GameObject.FindGameObjectWithTag("Player");
        powerUpManager.AddEntity(player);
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

        fpsSmoothing += Time.deltaTime;

        if(fpsSmoothing > 0.5f)
        {
            fpsSmoothing = 0;
            fpsCounterLabel.text = (1f / Time.unscaledDeltaTime).ToString("N0") + " FPS";
        }

        powerUpManager.UpdateDurations(Time.deltaTime);
        Debug.Log(powerUpManager.GetAllEntityPowerUps(player));
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

    public void AddPoints(float p)
    {
        points += p;
    }


    public void NotifyPowerUpTriggerEnter(Collider other, GameObject sender)
    {
        if(other.tag == "Player")
        {
            powerUpManager.AddPowerUpToEntity(other.gameObject, sender.GetComponent<PowerUpScript>().type);
            Destroy(sender);
        }
    }
}
