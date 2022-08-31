using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashScript : MonoBehaviour
{
    private Light light;
    private void Awake()
    {
        light = GetComponent<Light>();
    }

    public void StartFlash()
    {
        LTDescr leanOperation = ChangeIntensity(light.intensity, 100, 2);
        leanOperation.setOnComplete(() =>
        {
            LTDescr leanOperation2 = ChangeIntensity(light.intensity, 0, 2);
        });
    }

    private LTDescr ChangeIntensity(float from, float to, float time)
    {
        return LeanTween.value(from, to, time).setOnUpdate(value =>
        {
            light.intensity = value;
        });
    }
}
