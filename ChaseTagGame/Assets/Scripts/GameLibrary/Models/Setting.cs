using System;
using UnityEngine;

namespace GameLibrary
{
    public class Setting : ISetting
    {
        public SettingType Type { get; set; }
        public dynamic Value { get; set; }
        public dynamic DefaultValue { get; set; }
        private readonly Type valueType;

        public Setting(SettingType type)
        {
            Type = type;


            valueType = Type switch
            {
                SettingType.MouseSensitivity => typeof(float),
                _ => typeof(int),
            };
        }
        public Setting(SettingType type, dynamic defaultValue) : this(type)
        {
            DefaultValue = defaultValue;
        }


        public void Load()
        {
            var settingName = Type.ToString("g");
            var hasKey = PlayerPrefs.HasKey(settingName);

            if (valueType == typeof(float))
                Value = hasKey ? PlayerPrefs.GetFloat(settingName) : DefaultValue;
            else if (valueType == typeof(int))
                Value = hasKey ? PlayerPrefs.GetInt(settingName) : DefaultValue;
            else if (valueType == typeof(string))
                Value = hasKey ? PlayerPrefs.GetString(settingName) : DefaultValue;
        }

        public void Save(dynamic newValue)
        {
            if(valueType == typeof(float))
                PlayerPrefs.SetFloat(Type.ToString("g"), (float)Convert.ChangeType(newValue, typeof(float)));
            else if(valueType == typeof(int))
                PlayerPrefs.SetInt(Type.ToString("g"), (int)Convert.ChangeType(newValue, typeof(int)));
            else if(valueType == typeof(string))
                PlayerPrefs.SetString(Type.ToString("g"), (string)Convert.ChangeType(newValue, typeof(string)));
        }
    }


    public enum SettingType
    {
        MouseSensitivity
    }
}
