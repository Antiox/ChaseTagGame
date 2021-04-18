using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLibrary
{
    public class SettingsManager
    {
        private List<ISetting> settings;


        #region Singleton
        private static SettingsManager instance;
        public static SettingsManager Instance
        {
            get
            {
                instance ??= new SettingsManager();
                return instance;
            }
        }
        #endregion

        private SettingsManager()
        {

        }


        public void Start()
        {
            InitializeSettings();
            LoadSettings();
            EventManager.Instance.AddListener<OnSettingChangedEvent>(OnSettingChanged);
        }

        public void OnDestroy()
        {
            EventManager.Instance.RemoveListener<OnSettingChangedEvent>(OnSettingChanged);
        }

        public dynamic GetSettingValue(SettingType type)
        {
            return settings.Find(s => s.Type == type).Value;
        }


        private void LoadSettings()
        {
            foreach (var s in settings)
                s.Load();
        }

        private void InitializeSettings()
        {
            settings = new List<ISetting>
            {
                new Setting(SettingType.MouseSensitivity, 1f),
            };
        }


        private void OnSettingChanged(OnSettingChangedEvent e)
        {
            settings.Find(s => s.Type == e.Setting).Save(e.Value);
        }
    }
}
