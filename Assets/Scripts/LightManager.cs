using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : Singleton<LightManager>
{
    [SerializeField] private List<Light> statueLights;

    [SerializeField] private List<Light> fireLights;

    [SerializeField] private float minFireIntensity = 0;
    
    [SerializeField] private float maxFireIntensity = 1;
    
    [SerializeField] private float fireIntensityChangeSpeed = 1;

    private float _statueNormalIntensity = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        _statueNormalIntensity = statueLights[0].intensity;

        Timed.RunContinuous((f => FireIntensityChanger()),100,delay: 5);
    }

    public void StatueLightShine(float target, float duration)
    {
        StartCoroutine(Shiner());
        
        IEnumerator Shiner()
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float t = elapsed / duration;
                float intensity = Mathf.Lerp(_statueNormalIntensity, target, t);

                foreach (var statueLight in statueLights)
                {
                    statueLight.intensity = intensity;
                }

                elapsed += Time.deltaTime;
                yield return null;
            }
        }
    }

    public void FireIntensityChanger()
    {
        var intensity = NoiseBetween(minFireIntensity, maxFireIntensity, fireIntensityChangeSpeed);

        foreach (var fireLight in fireLights)
        {
            fireLight.intensity = intensity;
        }
    }
    
    public float NoiseBetween(float min, float max, float speed)
    {
        float time = Time.time * speed;
        float noise = Mathf.PerlinNoise(time, 0f); // 0 ile 1 arasında
        return Mathf.Lerp(min, max, noise);
    }

}
