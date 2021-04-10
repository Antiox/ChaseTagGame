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

            EventManager.Instance.AddListener<OnSkillBoughtEvent>(OnSkillBought);
        }

        public  void Update()
        {
            Debug.Log(Player.IsInvulnerable);

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

        public void OnDestroy()
        {
            EventManager.Instance.RemoveListener<OnSkillBoughtEvent>(OnSkillBought);
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
            EventManager.Instance.RemoveListener<OnSkillBoughtEvent>(OnSkillBought);
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

        public void TriggerLoseObjects()
        {
            var playerScript = Player.gameObject.GetComponent<MonoBehaviour>();
            Player.TriggerTemporaryInvulnerability();
            playerScript.StartCoroutine(LoseObjects(CurrentDay.ObjectsCollected));
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
                var spawnPosition = Utility.GetRandomNavMeshPosition() + Vector3.up;
                GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Key"), spawnPosition, Quaternion.identity);
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
                yield return new WaitForSeconds(0.4f / amount);
            }
        }


        private void OnSkillBought(OnSkillBoughtEvent e)
        {
            Currency = e.NewCurrency;
        }
    }
}
