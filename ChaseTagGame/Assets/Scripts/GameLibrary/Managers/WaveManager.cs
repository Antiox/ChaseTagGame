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
        public Player Player { get; private set; }
        public List<Enemy> Enemies { get; private set; }
        public int Currency { get; set; }

        private float endofDayMultiplier;


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
            Player = new Player();
            Enemies = new List<Enemy>();
        }

        public void Start()
        {
            Player = new Player(GameObject.Find("Player"));
            endofDayMultiplier = 1f;
            Enemies.Clear();
            SpawnEnemies();
            SpawnObjectives();
        }

        public  void Update()
        {
            CurrentDay.TimeLeft -= Time.deltaTime * endofDayMultiplier;
            var e1 = new OnTimeChangedEvent(CurrentDay.InitialTime, CurrentDay.TimeLeft);
            EventManager.Instance.Dispatch(e1);


            if (CurrentDay.TimeLeft <= 0)
            {
                Currency += CurrentDay.ObjectsCollected;
                var e2 = new OnDayEndedEvent();
                EventManager.Instance.Dispatch(e2);
            }
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
        }

        public void ExtendDuration(double amount)
        {
            CurrentDay.InitialTime += (amount * CurrentDay.InitialTime) / CurrentDay.TimeLeft;
            CurrentDay.TimeLeft += amount;
        }

        public bool IsPlayerInSafeZone()
        {
            return Player.IsInSafeZone;
        }

        public bool CollectedEnoughObjectives()
        {
            return CurrentDay.ObjectsCollected >= CurrentDay.RequiredObjects / 2f;
        }

        public void IncreaseCollectedObjectives()
        {
            CurrentDay.ObjectsCollected++;
        }

        public void AccelerateEndOfDay()
        {
            endofDayMultiplier = 20f;
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
    }
}
