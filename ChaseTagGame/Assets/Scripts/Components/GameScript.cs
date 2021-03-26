using GameLibrary;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameScript : MonoBehaviour
{
    public TextMeshProUGUI gamePausedLabel;
    public TextMeshProUGUI gameOverLabel;
    public TextMeshProUGUI pointCounterLabel;
    public TextMeshProUGUI fpsCounterLabel;

    private readonly PowerUpManager powerUpManager = PowerUpManager.Instance;
    private readonly EventManager eventManager = EventManager.Instance;

    private bool isPaused = false;
    private float points = 0;
    private float fpsSmoothing = 0;


    public void Awake()
    {
        Time.timeScale = 1f;

        eventManager.AddListener<OnPowerUpTriggerEnterEvent>(OnPowerUpTriggerEnter);
        eventManager.AddListener<OnEnemyTriggerEnterEvent>(OnEnemyTriggerEnter);
    }

    private void Update()
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

        powerUpManager.Update();
    }

    private void OnDestroy()
    {
        eventManager.RemoveListener<OnPowerUpTriggerEnterEvent>(OnPowerUpTriggerEnter);
        eventManager.RemoveListener<OnEnemyTriggerEnterEvent>(OnEnemyTriggerEnter);
        eventManager.RemoveListener<OnPointsAddedEvent>(OnPointsAdded);
    }

    public void GameOver()
    {
        gameOverLabel.enabled = true;
        Time.timeScale = 0.01f;
        Utility.Invoke(RestartGame, 3f * Time.timeScale);
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


    private void OnPowerUpTriggerEnter(OnPowerUpTriggerEnterEvent e)
    {
        if (e.Entity.CompareTag(GameTags.Player))
        {
            powerUpManager.AddPowerUpToEntity(e.Entity, e.PowerUp.GetComponent<PowerUpScript>().type);
            Destroy(e.PowerUp);
        }
    }

    private void OnEnemyTriggerEnter(OnEnemyTriggerEnterEvent e)
    {
        if (e.Entity.CompareTag(GameTags.Player))
        {
            if (powerUpManager.IsEntityShielded(e.Entity))
                powerUpManager.RemovePowerUpFromEntity(e.Entity, PowerUpType.Shield);
            else
                GameOver();
        }
    }

    private void OnPointsAdded(OnPointsAddedEvent e)
    {
        points += e.Points;
    }
}
