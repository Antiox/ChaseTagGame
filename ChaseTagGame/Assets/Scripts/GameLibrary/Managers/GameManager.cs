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
        public static HudManagerScript hudManager { get; private set; }
        public static GameObject mainGameObject { get; private set; }
        public static GameScript mainGameScript { get; private set; }
        public static GameState State { get; private set; }


        public static float Points { get; private set; }


        public static void Start()
        {
            waveManager.Start();

            waveManager.Players.Add(new Player(GameObject.Find("Player")));

            mainGameObject = GameObject.Find("GameManager");
            mainGameScript = mainGameObject.GetComponent<GameScript>();

            hudManager = mainGameObject.GetComponent<HudManagerScript>();

            eventManager.AddListener<OnPowerUpTriggerEnterEvent>(OnPowerUpTriggerEnter);
            eventManager.AddListener<OnEnemyTriggerEnterEvent>(OnEnemyTriggerEnter);
            eventManager.AddListener<OnPointsAddedEvent>(OnPointsAdded);
            eventManager.AddListener<OnDayEndedEvent>(OnDayEnded);
            eventManager.AddListener<OnTimeAddedEvent>(OnTimeAdded);
            eventManager.AddListener<OnPlayerExitSafeZoneEvent>(OnPlayerExitSafeZone);

            State = GameState.WaitingPlayer;
        }

        public static void Update()
        {
            if (State == GameState.InGame)
            {
                powerUpManager.Update();
                waveManager.Update();
            }

            hudManager.DisplayDayInfo(waveManager.CurrentDay);
        }

        public static void OnDestroy()
        {
            waveManager.OnDestroy();
            eventManager.RemoveListener<OnPowerUpTriggerEnterEvent>(OnPowerUpTriggerEnter);
            eventManager.RemoveListener<OnEnemyTriggerEnterEvent>(OnEnemyTriggerEnter);
            eventManager.RemoveListener<OnPointsAddedEvent>(OnPointsAdded);
            eventManager.RemoveListener<OnDayEndedEvent>(OnDayEnded);
            eventManager.RemoveListener<OnPlayerExitSafeZoneEvent>(OnPlayerExitSafeZone);
        }

        public static void Reset()
        {
            waveManager.Reset();
        }


        private static void GameOver()
        {
            State = GameState.GameOver;
            hudManager.DisplayGameOver();
            mainGameScript.Invoke("GoToMainMenu", 3f);
        }


        private static void OnPowerUpTriggerEnter(OnPowerUpTriggerEnterEvent e)
        {
            if (e.Entity.CompareTag(GameTags.Player))
            {
                var powerUpComponent = e.PowerUp.GetComponent<PowerUpScript>();
                var p = powerUpComponent.powerUp;
                powerUpManager.AddPowerUpToEntity(e.Entity, p);
                UnityEngine.Object.Destroy(e.PowerUp);
            }
            else if (e.Entity.CompareTag(GameTags.Enemy))
            {
                UnityEngine.Object.Destroy(e.PowerUp);
            }
        }

        private static void OnEnemyTriggerEnter(OnEnemyTriggerEnterEvent e)
        {
            if (e.Entity.CompareTag(GameTags.Player) && State == GameState.InGame)
            {
                if (powerUpManager.IsEntityShielded(e.Entity))
                    powerUpManager.RemovePowerUpFromEntity(e.Entity, PowerUpType.Shield);
                else
                    GameOver();
            }
        }

        private static void OnPointsAdded(OnPointsAddedEvent e)
        {
            Points += e.Points;
        }

        private static void OnDayEnded(OnDayEndedEvent e)
        {
            if(waveManager.ArePlayersInSafeZone())
            {
                State = GameState.DayEnded;
                hudManager.DisplayEndOfDay();
                waveManager.LoadNextDay();
                mainGameScript.Invoke("RestartGame", 3f);
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
    }
}
