using GameLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class DayNightCycleScript : MonoBehaviour
{
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;

    [SerializeField, Range(6f, 19f)] private float TimeOfDay;


    void Start()
    {
        EventManager.Instance.AddListener<OnTimeChangedEvent>(OnTimeChanged);
    }

    void OnDestroy()
    {
        EventManager.Instance.RemoveListener<OnTimeChangedEvent>(OnTimeChanged);
    }

    void Update()
    {
        if (Preset == null)
            return;

        UpdateLighting(TimeOfDay / 24f);
    }


    private void UpdateLighting(float timePercent)
    {
        //Set ambient and fog
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);
        DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);
        DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
    }



    private void OnTimeChanged(OnTimeChangedEvent e)
    {
        TimeOfDay = 6f + ((e.InitialTime - e.TimeLeft) / e.InitialTime) * 13f;
    }
}
