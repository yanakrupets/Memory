using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class Brightness : MonoBehaviour
{
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private PostProcessProfile brightness;
    [SerializeField] private PostProcessLayer layer;

    private AutoExposure exposure;

    public void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        brightness.TryGetSettings(out exposure);
        AdjustBrightness(brightnessSlider.value);
    }

    public void AdjustBrightness(float value)
    {
        exposure.keyValue.value = 0.5f + value * 0.05f;
        GameSettings.brightnessValue = exposure.keyValue.value;
    }
}
