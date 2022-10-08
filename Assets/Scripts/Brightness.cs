using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class Brightness : MonoBehaviour
{
    [SerializeField] private PostProcessProfile profile;
    [SerializeField] private PostProcessLayer layer;

    private AutoExposure exposure;

    void Start()
    {
        profile.TryGetSettings(out exposure);
        exposure.keyValue.value = PlayerPrefs.GetFloat("Brightness", 10) / 10;
    }

    public void AdjustBrightness(float value)
    {
        PlayerPrefs.SetFloat("Brightness", value);
        exposure.keyValue.value = 0.5f + value * 0.05f;
    }
}
