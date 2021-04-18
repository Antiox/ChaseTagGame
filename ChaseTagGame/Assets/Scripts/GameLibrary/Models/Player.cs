using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLibrary
{
    public class Player
    {
        public GameObject gameObject { get; set; }
        public bool IsInSafeZone { get; set; }
        public bool IsInvulnerable { get; set; }
        public float InvulnerabilityDuration { get; set; } = 3f;
        public Vector3 Direction  { get { return movementScript.PlayerDirection; } }

        private readonly PlayerMovement movementScript;


        public Player() 
        {
        }
        public Player(GameObject o)
        {
            gameObject = o;
            EventManager.Instance.AddListener<OnPlayerEnterSafeZoneEvent>(OnPlayerEnterSafeZone);
            EventManager.Instance.AddListener<OnPlayerExitSafeZoneEvent>(OnPlayerExitSafeZone);
            movementScript = gameObject.GetComponent<PlayerMovement>();
        }

        ~Player()
        {
            EventManager.Instance.RemoveListener<OnPlayerEnterSafeZoneEvent>(OnPlayerEnterSafeZone);
            EventManager.Instance.RemoveListener<OnPlayerExitSafeZoneEvent>(OnPlayerExitSafeZone);
        }

        public void TriggerTemporaryInvulnerability()
        {
            gameObject.GetComponent<MonoBehaviour>().StartCoroutine(InvulnerabilityFrames());
        }


        private IEnumerator InvulnerabilityFrames()
        {
            var renderers = gameObject.GetComponentsInChildren<Renderer>();
            var originalColors = new Color[renderers.Length];

            for (int i = 0; i < renderers.Length; i++)
            {
                originalColors[i] = renderers[i].material.color;
                renderers[i].material.color = Color.blue;
            }

            IsInvulnerable = true;
            yield return new WaitForSeconds(InvulnerabilityDuration);
            IsInvulnerable = false;

            for (int i = 0; i < renderers.Length; i++)
                renderers[i].material.color = originalColors[i];
        }


        private void OnPlayerExitSafeZone(OnPlayerExitSafeZoneEvent e)
        {
            if (e.Player == gameObject)
                IsInSafeZone = false;
        }

        private void OnPlayerEnterSafeZone(OnPlayerEnterSafeZoneEvent e)
        {
            if (e.Player == gameObject)
                IsInSafeZone = true;
        }
    }
}
