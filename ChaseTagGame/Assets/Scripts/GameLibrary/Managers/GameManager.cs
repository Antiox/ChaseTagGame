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
        public static PowerUpManager PowerUpManager { get; private set; } = PowerUpManager.Instance;
        public static EventManager EventManager { get; private set; } = EventManager.Instance;
        public static WaveManager WaveManager { get; private set; } = WaveManager.Instance;
        public static SettingsManager SettingsManager { get; private set; } = SettingsManager.Instance;
        public static SkillsManager SkillsManager { get; private set; } = SkillsManager.Instance;
        public static HudManagerScript HudManager { get; private set; }
        public static GameObject MainGameObject { get; private set; }
        public static GameScript MainGameScript { get; private set; }
        public static GameState State { get; private set; }


        public static float Points { get; private set; }

        private static GameState previousState;



        public static void Start()
        {
            State = SceneManager.GetActiveScene().buildIndex == 0 ? GameState.MainMenu : GameState.WaitingPlayer;

            AddListeners();
            SettingsManager.Start();


            if (State != GameState.MainMenu)
            {
                WaveManager.Start();
                SkillsManager.Start();

                MainGameObject = GameObject.Find("GameManager");
                MainGameScript = MainGameObject.GetComponent<GameScript>();
                HudManager = MainGameObject.GetComponent<HudManagerScript>();

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        public static void AddListeners()
        {
            EventManager.AddListener<OnPowerUpTriggerEnterEvent>(OnPowerUpTriggerEnter);
            EventManager.AddListener<OnEnemyTriggerEnterEvent>(OnEnemyTriggerEnter);
            EventManager.AddListener<OnPointsAddedEvent>(OnPointsAdded);
            EventManager.AddListener<OnDayEndedEvent>(OnDayEnded);
            EventManager.AddListener<OnTimeAddedEvent>(OnTimeAdded);
            EventManager.AddListener<OnPlayerExitSafeZoneEvent>(OnPlayerExitSafeZone);
            EventManager.AddListener<OnObjectiveItemTriggerEnterEvent>(OnObjectiveItemTriggerEnter);
            EventManager.AddListener<OnInteractiveElementPressedEvent>(OnInteractiveElementPressed);
            EventManager.AddListener<OnGemTriggerEnterEvent>(OnGemTriggerEnter);
        }

        public static void Update()
        {
            if (State == GameState.InGame)
            {
                PowerUpManager.Update();
                WaveManager.Update();
            }

            if(State != GameState.MainMenu)
            {
                HandlePauseGame();
                HudManager.DisplayDayInfo(WaveManager.CurrentDay);
            }
        }

        public static void OnDestroy()
        {
            SettingsManager.OnDestroy();
            SkillsManager.OnDestroy();
            WaveManager.OnDestroy();
            EventManager.RemoveListener<OnPowerUpTriggerEnterEvent>(OnPowerUpTriggerEnter);
            EventManager.RemoveListener<OnEnemyTriggerEnterEvent>(OnEnemyTriggerEnter);
            EventManager.RemoveListener<OnPointsAddedEvent>(OnPointsAdded);
            EventManager.RemoveListener<OnDayEndedEvent>(OnDayEnded);
            EventManager.RemoveListener<OnPlayerExitSafeZoneEvent>(OnPlayerExitSafeZone);
            EventManager.RemoveListener<OnInteractiveElementPressedEvent>(OnInteractiveElementPressed);
            EventManager.RemoveListener<OnObjectiveItemTriggerEnterEvent>(OnObjectiveItemTriggerEnter);
            EventManager.RemoveListener<OnGemTriggerEnterEvent>(OnGemTriggerEnter);
        }

        public static void Reset()
        {
            WaveManager.Reset();
            SkillsManager.Reset();
            OnDestroy();
        }


        private static void GameOver()
        {
            State = GameState.GameOver;
            HudManager.DisplayGameOver(WaveManager.CurrentDay);
            
            var e = new OnGameOverEvent(WaveManager.CurrentDay);
            EventManager.Instance.Dispatch(e);
        }

        private static void PauseGame()
        {
            previousState = State;
            State = GameState.Paused;
            HudManager.PauseGame();
            Time.timeScale = 0;
        }

        public static void ResumeGame()
        {
            State = previousState;
            HudManager.ResumeGame();
            Time.timeScale = 1f;
        }

        public static void GoToNextDay()
        {
            WaveManager.LoadNextDay();
            MainGameScript.RestartGame();
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
            WaveManager.TriggerLoseObjects();
        }

        public static bool IsOwningSkill(SkillType type)
        {
            return SkillsManager.IsOwningSkill(type);
        }




        private static void OnPowerUpTriggerEnter(OnPowerUpTriggerEnterEvent e)
        {
            var powerUpComponent = e.PowerUp.GetComponent<PowerUpScript>();
            var p = powerUpComponent.powerUp;
            PowerUpManager.AddPowerUpToEntity(e.Entity, p);
            UnityEngine.Object.Destroy(e.PowerUp);
        }

        private static void OnEnemyTriggerEnter(OnEnemyTriggerEnterEvent e)
        {
            if (e.Entity.CompareTag(GameTags.Player) && State == GameState.InGame && !WaveManager.Player.IsInvulnerable)
            {
                if (PowerUpManager.IsEntityShielded(e.Entity))
                    PowerUpManager.RemovePowerUpFromEntity(e.Entity, PowerUpType.Shield);
                else if(WaveManager.CurrentDay.ObjectsCollected == 0)
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
            if(WaveManager.IsPlayerInSafeZone() && WaveManager.CollectedEnoughObjectives())
            {
                State = GameState.Shopping;
                HudManager.DisplaySkills(SkillsManager.PossibleSkills);
                HudManager.DisplayShopMenu();
                HudManager.DisplayCurrencyAmount(WaveManager.Currency);
            }
            else
            {
                GameOver();
            }
        }

        private static void OnTimeAdded(OnTimeAddedEvent e)
        {
            WaveManager.ExtendDayDuration(e.Amount);
        }

        private static void OnPlayerExitSafeZone(OnPlayerExitSafeZoneEvent e)
        {
            State = GameState.InGame;
        }

        private static void OnObjectiveItemTriggerEnter(OnObjectiveItemTriggerEnterEvent e)
        {
            WaveManager.IncreaseCollectedObjectives();
            GameObject.Destroy(e.Objective.gameObject);
        }

        private static void OnGemTriggerEnter(OnGemTriggerEnterEvent e)
        {
            WaveManager.IncreaseCollectedGems();
            GameObject.Destroy(e.Gem.gameObject);
        }

        private static void OnInteractiveElementPressed(OnInteractiveElementPressedEvent e)
        {
            if(e.Action == ActionType.EndDay)
                WaveManager.AccelerateEndOfDay();
        }
    }
}
