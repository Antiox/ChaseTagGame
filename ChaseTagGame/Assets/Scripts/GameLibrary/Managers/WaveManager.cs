using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLibrary
{
    public class WaveManager
    {
        public DayInfo CurrentDay { get; private set; }
        public List<Player> Players { get; private set; }
        public List<Enemy> Enemies { get; private set; }


        #region Singleton
        private static WaveManager instance;
        public static WaveManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new WaveManager();
                return instance;
            }
        }
        #endregion


        private WaveManager()
        {
            CurrentDay = new DayInfo();
            Players = new List<Player>();
            Enemies = new List<Enemy>();
        }

        public void Start()
        {
            Players.Clear();
            Enemies.Clear();

            EventManager.Instance.AddListener<OnPlayerEnterSafeZoneEvent>(OnPlayerEnterSafeZone);
            EventManager.Instance.AddListener<OnPlayerExitSafeZoneEvent>(OnPlayerExitSafeZone);

            SpawnEnemies();
            SpawnObjectives();
        }

        public  void Update()
        {
            CurrentDay.TimeLeft -= Time.deltaTime;
            var e1 = new OnTimeChangedEvent(CurrentDay.InitialTime, CurrentDay.TimeLeft);
            EventManager.Instance.Dispatch(e1);


            if (CurrentDay.TimeLeft <= 0)
            {
                var e2 = new OnDayEndedEvent();
                EventManager.Instance.Dispatch(e2);
            }
        }

        public void OnDestroy()
        {
            EventManager.Instance.RemoveListener<OnPlayerEnterSafeZoneEvent>(OnPlayerEnterSafeZone);
            EventManager.Instance.RemoveListener<OnPlayerExitSafeZoneEvent>(OnPlayerExitSafeZone);
        }


        public void LoadNextDay()
        {
            var missingObjects = CurrentDay.RequiredObjects - CurrentDay.ObjectsCollected;
            CurrentDay = new DayInfo(CurrentDay.Number + 1);
            CurrentDay.RequiredObjects += missingObjects;
        }

        public void Reset()
        {
            CurrentDay = new DayInfo();
            Enemies.Clear();
            Players.Clear();
        }

        public void ExtendDuration(double amount)
        {
            CurrentDay.InitialTime += (amount * CurrentDay.InitialTime) / CurrentDay.TimeLeft;
            CurrentDay.TimeLeft += amount;
        }

        public bool ArePlayersInSafeZone()
        {
            foreach (var p in Players)
                if (!p.IsInSafeZone)
                    return false;

            return true;
        }

        public bool CollectedEnoughObjectives()
        {
            return CurrentDay.ObjectsCollected >= CurrentDay.RequiredObjects / 2f;
        }

        public void IncreaseCollectedObjectives()
        {
            CurrentDay.ObjectsCollected++;
        }


        private void SpawnEnemies()
        {
            for (int i = 0; i < CurrentDay.Number; i++)
            {
                var enemy = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Enemy"), Utility.GetRandomNavMeshPosition(), Quaternion.identity);
                Enemies.Add(new Enemy(enemy));
            }
        }

        private void SpawnObjectives()
        {
            for (int i = 0; i < (CurrentDay.RequiredObjects + 1); i++)
            {
                GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Key"), Utility.GetRandomNavMeshPosition(), Quaternion.identity);
            }
        }


        private void OnPlayerExitSafeZone(OnPlayerExitSafeZoneEvent e)
        {
            var player = Players.Find(p => p.gameObject == e.Player);
            player.IsInSafeZone = false;
        }

        private void OnPlayerEnterSafeZone(OnPlayerEnterSafeZoneEvent e)
        {
            var player = Players.Find(p => p.gameObject == e.Player);
            player.IsInSafeZone = true;
        }
    }
}
