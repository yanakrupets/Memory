using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class Brightness : MonoBehaviour
{
    [SerializeField] private PostProcessProfile brightness;
    [SerializeField] private PostProcessLayer layer;

    private AutoExposure exposure;

    void Start()
    {
        brightness.TryGetSettings(out exposure);
        exposure.keyValue.value = 1;
    }

    public void AdjustBrightness(float value)
    {
        PlayerPrefs.SetFloat("Brightness", value);
        exposure.keyValue.value = 0.5f + value * 0.05f;
    }
}
