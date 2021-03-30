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
        public DayInfo CurrentDay { get; set; }
        public List<Player> Players { get; set; }
        public List<Enemy> Enemies { get; set; }

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
        }

        public  void Update()
        {
            CurrentDay.TimeLeft -= Time.deltaTime;

            if(CurrentDay.TimeLeft <= 0)
            {
                var e = new OnDayEndedEvent();
                EventManager.Instance.Broadcast(e);
            }
        }

        public void OnDestroy()
        {
            EventManager.Instance.RemoveListener<OnPlayerEnterSafeZoneEvent>(OnPlayerEnterSafeZone);
            EventManager.Instance.RemoveListener<OnPlayerExitSafeZoneEvent>(OnPlayerExitSafeZone);
        }


        public void LoadNextDay()
        {
            CurrentDay = new DayInfo(CurrentDay.Number + 1);
        }

        public void Reset()
        {
            CurrentDay = new DayInfo();
            Enemies.Clear();
            Players.Clear();
        }

        public void ExtendDuration(double amount)
        {
            CurrentDay.TimeLeft += amount;
        }

        public bool ArePlayersInSafeZone()
        {
            foreach (var p in Players)
                if (!p.IsInSafeZone)
                    return false;

            return true;
        }


        private void SpawnEnemies()
        {
            for (int i = 0; i < CurrentDay.Number; i++)
            {
                var enemy = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Enemy"), Utility.GetRandomNavMeshPosition(), Quaternion.identity);
                Enemies.Add(new Enemy(enemy));
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
