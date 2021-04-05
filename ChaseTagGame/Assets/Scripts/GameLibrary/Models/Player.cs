using System;
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


        public Player() 
        {

        }
        public Player(GameObject o)
        {
            gameObject = o;
            EventManager.Instance.AddListener<OnPlayerEnterSafeZoneEvent>(OnPlayerEnterSafeZone);
            EventManager.Instance.AddListener<OnPlayerExitSafeZoneEvent>(OnPlayerExitSafeZone);
        }

        ~Player()
        {
            EventManager.Instance.RemoveListener<OnPlayerEnterSafeZoneEvent>(OnPlayerEnterSafeZone);
            EventManager.Instance.RemoveListener<OnPlayerExitSafeZoneEvent>(OnPlayerExitSafeZone);
        }

        private void OnPlayerExitSafeZone(OnPlayerExitSafeZoneEvent e)
        {
            if(e.Player == gameObject)
                IsInSafeZone = false;
        }

        private void OnPlayerEnterSafeZone(OnPlayerEnterSafeZoneEvent e)
        {
            if (e.Player == gameObject)
                IsInSafeZone = true;
        }
    }
}
