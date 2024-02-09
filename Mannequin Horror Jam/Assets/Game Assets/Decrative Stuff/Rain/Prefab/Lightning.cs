using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public GameObject targetObject; // The object to be lit up
    public float minFlashDelay = 0.1f; // Minimum delay between flashes
    public float maxFlashDelay = 1f; // Maximum delay between flashes
    public float minIntensity = 0.1f; // Minimum light intensity
    public float maxIntensity = 1f; // Maximum light intensity

    private Light lightningLight;
    private float nextFlashTime;

    void Start()
    {
        lightningLight = targetObject.GetComponent<Light>();
        if (lightningLight == null)
        {
            Debug.LogError("No light component found on the target object.");
            enabled = false;
            return;
        }

        nextFlashTime = Time.time + Random.Range(minFlashDelay, maxFlashDelay);
    }

    void Update()
    {
        if (Time.time >= nextFlashTime)
        {
            Flash();
            nextFlashTime = Time.time + Random.Range(minFlashDelay, maxFlashDelay);
        }
    }

    void Flash()
    {
        float intensity = Random.Range(minIntensity, maxIntensity);
        lightningLight.intensity = intensity;
        Invoke("ResetLightIntensity", 0.05f); // Reset light intensity after a short delay
    }

    void ResetLightIntensity()
    {
        lightningLight.intensity = 0f; // Turn off the light
    }
}
