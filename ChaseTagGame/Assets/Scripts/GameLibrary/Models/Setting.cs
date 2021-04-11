using System;
using UnityEngine;

namespace GameLibrary
{
    public class Setting : ISetting
    {
        public SettingType Type { get; set; }
        public dynamic Value { get; set; }
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

        public void Load()
        {
            if (valueType == typeof(float))
                Value = PlayerPrefs.GetFloat(Type.ToString("g"));
            else if (valueType == typeof(int))
                Value = PlayerPrefs.GetInt(Type.ToString("g"));
            else if (valueType == typeof(string))
                Value = PlayerPrefs.GetString(Type.ToString("g"));
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
