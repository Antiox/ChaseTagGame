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

        public void LoadNextDay()
        {
            CurrentDay = new DayInfo(CurrentDay.Number + 1);
        }

        public void Reset()
        {
            CurrentDay = new DayInfo();
        }

        public void ExtendDuration(double amount)
        {
            CurrentDay.TimeLeft += amount;
        }
    }
}
