using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLibrary
{
    public class SettingsManager : Singleton<SettingsManager>
    {
        public void Start()
        {
            EventManager.Instance.AddListener<OnSettingChangedEvent>(OnSettingChanged);
        }

        public void OnDestroy()
        {
            EventManager.Instance.RemoveListener<OnSettingChangedEvent>(OnSettingChanged);
        }

        private static void OnSettingChanged(OnSettingChangedEvent e)
        {
            switch (e.Setting)
            {
                case Settings.MouseSensitivity: SaveMouseSensitivity(e); break;
            }
        }

        private static void SaveMouseSensitivity(OnSettingChangedEvent e)
        {
            PlayerPrefs.SetFloat(e.Setting.ToString("g"), (float)e.Value);
        }
    }
}
