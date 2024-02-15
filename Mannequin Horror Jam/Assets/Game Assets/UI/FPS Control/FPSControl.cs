using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class FPSControl : MonoBehaviour
{
    public float updateInterval = 0.5f; // How often, in seconds, the FPS should be updated
    private float accum = 0.0f; // FPS accumulated over the interval
    private int frames = 0; // Frames drawn over the interval
    private float timeleft; // Left time for current interval

    public TextMeshProUGUI fpsText; // Reference to the UI Text element

    void Start()
    {
        if (updateInterval <= 0.0f)
        {
            updateInterval = 0.5f;
        }

        timeleft = updateInterval;

        // Find the Text component in the UI
        //fpsText = GetComponent<TextMeshProUGUI>();
        if (fpsText == null)
        {
            Debug.LogError("FPSCounter: UI Text component not found. Make sure to attach this script to a UI Text element.");
            enabled = false;
        }
    }

    void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        // Interval ended - update FPS and start a new interval
        if (timeleft <= 0.0f)
        {
            // Display the FPS on the UI Text element
            float fps = accum / frames;
            fpsText.text = string.Format("{0:F2} FPS", fps);

            timeleft = updateInterval;
            accum = 0.0f;
            frames = 0;
        }
    }
}
