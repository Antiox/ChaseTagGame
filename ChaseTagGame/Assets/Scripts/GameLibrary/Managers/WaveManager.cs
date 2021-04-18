using System;
using System.Collections;
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
        public List<IEnemy> Enemies { get; private set; }
        public int Currency { get; set; }

        private float endofDayMultiplier;


        #region Singleton
        private static WaveManager instance;
        public static WaveManager Instance
        {
            get
            {
                instance ??= new WaveManager();
                return instance;
            }
        }
        #endregion


        private WaveManager()
        {
            CurrentDay = new DayInfo();
            Player = new Player();
            Enemies = new List<IEnemy>();
        }

        public void Start()
        {
            Player = new Player(GameObject.Find("Player"));
            endofDayMultiplier = 1f;
            Enemies.Clear();
            SpawnEnemies();
            SpawnObjectives();
            SpawnGems();

        }

        public  void Update()
        {
            CurrentDay.TimeLeft -= Time.deltaTime * endofDayMultiplier;
            var e1 = new OnTimeChangedEvent(CurrentDay.InitialTime, CurrentDay.TimeLeft);
            EventManager.Instance.Dispatch(e1);


            if (CurrentDay.TimeLeft <= 0)
            {
                Currency += CurrentDay.GemsCollected;
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
            Currency = 0;
            CurrentDay = new DayInfo();
            Enemies.Clear();
        }

        public void ExtendDayDuration(double amount)
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

        public void IncreaseCollectedGems()
        {
            CurrentDay.GemsCollected++;
        }

        public void AccelerateEndOfDay()
        {
            endofDayMultiplier = 20f;
        }

        public void TriggerLoseObjects()
        {
            var playerScript = Player.gameObject.GetComponent<MonoBehaviour>();
            playerScript.StartCoroutine(LoseObjects(CurrentDay.ObjectsCollected));
            Player.TriggerTemporaryInvulnerability();
        }

        public IEnemy GetEnemyFromGameObject(GameObject o)
        {
            return Enemies.Find(e => e.GameObject == o);
        }



        private void SpawnEnemies()
        {
            for (int i = 0; i < CurrentDay.Number; i++)
            {
                var enemyGameObject = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Enemy"), Utility.GetRandomNavMeshPosition(), Quaternion.identity);
                var range = UnityEngine.Random.Range(15f, 40f);
                var angle = UnityEngine.Random.Range(40f, 140f);
                var enemy = new Enemy(enemyGameObject, angle, range);
                Enemies.Add(enemy);
            }
        }

        private void SpawnObjectives()
        {
            for (int i = 0; i < (CurrentDay.RequiredObjects + 1); i++)
            {
                var spawnPosition = Utility.GetRandomNavMeshPosition() + Vector3.up;
                GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Key"), spawnPosition, Quaternion.identity);
            }
        }

        private void SpawnGems()
        {
            for (int i = 0; i < 10; i++)
            {
                var spawnPosition = Utility.GetRandomNavMeshPosition() + Vector3.up;
                GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Gem"), spawnPosition, Quaternion.identity);
            }
        }

        private IEnumerator LoseObjects(float amount)
        {
            for (float i = 0f; i < amount; i++)
            {
                var spreadFactor = 10f;
                var spreadX = Vector3.forward * UnityEngine.Random.Range(-spreadFactor, spreadFactor);
                var spreadZ = Vector3.left * UnityEngine.Random.Range(-spreadFactor, spreadFactor);
                var upDirection = Vector3.up * 30f;
                var spawnPosition = Player.gameObject.transform.position + Vector3.up * 3f;

                var key = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Key"), spawnPosition , Quaternion.identity);
                var rigidbody = key.GetComponent<Rigidbody>();
                rigidbody.useGravity = true;
                rigidbody.AddForce(spreadX + spreadZ + upDirection, ForceMode.VelocityChange);
                rigidbody.AddTorque(Utility.GetRandomTorque(180f), ForceMode.VelocityChange);
                CurrentDay.ObjectsCollected--;
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}
