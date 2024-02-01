using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light flickeringLight;
    public float minIntensity = 0.5f;
    public float maxIntensity = 1.5f;
    public float flickerSpeed = 1f;

    public bool insteadUpdate;

    private void Start()
    {
        if (flickeringLight == null)
        {
            flickeringLight = GetComponent<Light>();
        }

        if (!insteadUpdate)
        {
            // Start the flickering coroutine
            StartCoroutine(Flicker());
        }

    }

    private void Update()
    {
        if (insteadUpdate)
        {
            FlickerUpdate();
        }
    }

    void FlickerUpdate()
    {

        // Generate a random intensity within the specified range
        float randomIntensity = Random.Range(minIntensity, maxIntensity);

        flickeringLight.intensity = Mathf.Lerp(flickeringLight.intensity, randomIntensity, flickerSpeed);

    }

    private IEnumerator Flicker()
    {
        while (true)
        {
            // Generate a random intensity within the specified range
            float randomIntensity = Random.Range(minIntensity, maxIntensity);

            // Smoothly lerp the light intensity towards the random intensity
            float elapsedTime = 0f;
            while (elapsedTime < flickerSpeed)
            {
                flickeringLight.intensity = Mathf.Lerp(flickeringLight.intensity, randomIntensity, elapsedTime / flickerSpeed);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Wait for a short time before the next flicker
            yield return new WaitForSeconds(0.001f);
        }
    }
}
