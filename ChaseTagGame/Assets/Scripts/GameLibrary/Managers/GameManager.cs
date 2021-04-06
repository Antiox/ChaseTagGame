using GameLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameLibrary
{
    public static class GameManager
    {
        public static PowerUpManager powerUpManager { get; private set; } = PowerUpManager.Instance;
        public static EventManager eventManager { get; private set; } = EventManager.Instance;
        public static WaveManager waveManager { get; private set; } = WaveManager.Instance;
        public static SettingsManager settingsManager { get; private set; } = SettingsManager.Instance;
        public static HudManagerScript hudManager { get; private set; }
        public static GameObject mainGameObject { get; private set; }
        public static GameScript mainGameScript { get; private set; }
        public static GameState State { get; private set; }


        public static float Points { get; private set; }

        private static GameState previousState;


        public static void Start()
        {
            waveManager.Start();
            settingsManager.Start();


            mainGameObject = GameObject.Find("GameManager");
            mainGameScript = mainGameObject.GetComponent<GameScript>();

            hudManager = mainGameObject.GetComponent<HudManagerScript>();

            eventManager.AddListener<OnPowerUpTriggerEnterEvent>(OnPowerUpTriggerEnter);
            eventManager.AddListener<OnEnemyTriggerEnterEvent>(OnEnemyTriggerEnter);
            eventManager.AddListener<OnPointsAddedEvent>(OnPointsAdded);
            eventManager.AddListener<OnDayEndedEvent>(OnDayEnded);
            eventManager.AddListener<OnTimeAddedEvent>(OnTimeAdded);
            eventManager.AddListener<OnPlayerExitSafeZoneEvent>(OnPlayerExitSafeZone);
            eventManager.AddListener<OnObjectiveItemTriggerEnterEvent>(OnObjectiveItemTriggerEnter);
            eventManager.AddListener<OnInteractiveElementPressedEvent>(OnInteractiveElementPressed);

            State = GameState.WaitingPlayer;
        }


        public static void Update()
        {
            if (State == GameState.InGame)
            {
                powerUpManager.Update();
                waveManager.Update();
            }
            else if(State == GameState.Shopping)
            {

            }

            HandlePauseGame();
            hudManager.DisplayDayInfo(waveManager.CurrentDay);
        }

        public static void OnDestroy()
        {
            settingsManager.OnDestroy();
            eventManager.RemoveListener<OnPowerUpTriggerEnterEvent>(OnPowerUpTriggerEnter);
            eventManager.RemoveListener<OnEnemyTriggerEnterEvent>(OnEnemyTriggerEnter);
            eventManager.RemoveListener<OnPointsAddedEvent>(OnPointsAdded);
            eventManager.RemoveListener<OnDayEndedEvent>(OnDayEnded);
            eventManager.RemoveListener<OnPlayerExitSafeZoneEvent>(OnPlayerExitSafeZone);
            eventManager.RemoveListener<OnInteractiveElementPressedEvent>(OnInteractiveElementPressed);
            eventManager.RemoveListener<OnObjectiveItemTriggerEnterEvent>(OnObjectiveItemTriggerEnter);
        }

        public static void Reset()
        {
            waveManager.Reset();
        }


        private static void GameOver()
        {
            State = GameState.GameOver;
            hudManager.DisplayGameOver();
            
            var e = new OnGameOverEvent(waveManager.CurrentDay);
            EventManager.Instance.Dispatch(e);
        }

        private static void PauseGame()
        {
            previousState = State;
            State = GameState.Paused;
            hudManager.PauseGame();
            Time.timeScale = 0;
        }

        public static void ResumeGame()
        {
            State = previousState;
            hudManager.ResumeGame();
            Time.timeScale = 1f;
        }

        public static void GoToNextDay()
        {
            waveManager.LoadNextDay();
            mainGameScript.RestartGame();
        }

        private static void HandlePauseGame()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (State != GameState.Paused && State != GameState.GameOver)
                    PauseGame();
                else
                    ResumeGame();
            }
        }

        private static void LoseObjects()
        {
            waveManager.TriggerLoseObjects();
        }




        private static void OnPowerUpTriggerEnter(OnPowerUpTriggerEnterEvent e)
        {
            var powerUpComponent = e.PowerUp.GetComponent<PowerUpScript>();
            var p = powerUpComponent.powerUp;
            powerUpManager.AddPowerUpToEntity(e.Entity, p);
            UnityEngine.Object.Destroy(e.PowerUp);
        }

        private static void OnEnemyTriggerEnter(OnEnemyTriggerEnterEvent e)
        {
            if (e.Entity.CompareTag(GameTags.Player) && State == GameState.InGame)
            {
                if (powerUpManager.IsEntityShielded(e.Entity))
                    powerUpManager.RemovePowerUpFromEntity(e.Entity, PowerUpType.Shield);
                else if(waveManager.CurrentDay.ObjectsCollected == 0)
                    GameOver();
                else
                    LoseObjects();
            }
        }

        private static void OnPointsAdded(OnPointsAddedEvent e)
        {
            Points += e.Points;
        }

        private static void OnDayEnded(OnDayEndedEvent e)
        {
            if(waveManager.IsPlayerInSafeZone() && waveManager.CollectedEnoughObjectives())
            {
                State = GameState.Shopping;
                hudManager.DisplayShopMenu();
                hudManager.DisplayCurrencyAmount(waveManager.Currency);
            }
            else
            {
                GameOver();
            }
        }

        private static void OnTimeAdded(OnTimeAddedEvent e)
        {
            waveManager.ExtendDuration(e.Amount);
        }

        private static void OnPlayerExitSafeZone(OnPlayerExitSafeZoneEvent e)
        {
            State = GameState.InGame;
        }

        private static void OnObjectiveItemTriggerEnter(OnObjectiveItemTriggerEnterEvent e)
        {
            waveManager.IncreaseCollectedObjectives();
            GameObject.Destroy(e.Objective.gameObject);
        }

        private static void OnInteractiveElementPressed(OnInteractiveElementPressedEvent e)
        {
            if(e.Action == ActionType.EndDay)
                waveManager.AccelerateEndOfDay();
        }
    }
}
