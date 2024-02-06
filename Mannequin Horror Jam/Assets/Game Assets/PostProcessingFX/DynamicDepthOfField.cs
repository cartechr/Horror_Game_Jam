using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessing : MonoBehaviour
{
    [Header("Global Post-PProcessing Depth of Field")]
    [Tooltip("Assign Global Volume Here")]
    public VolumeProfile volumeProfile;
    [Tooltip("Set the focal length here")]
    public float focalLength;
    [Tooltip("Set the focus distance length here")]
    public float focusDistance;
    [Tooltip("Maximum distance to cast the ray for focusing")]
    public float maxRaycastDistance = 100f;

    [Header("Assignments")]
    public Camera mainCamera;
    public LayerMask layerMask;

    DepthOfField depthOfFieldValue;



    void Start()
    {
        DepthOfField depthOfField;
        if (volumeProfile.TryGet<DepthOfField>(out depthOfField))
        {
            depthOfFieldValue = depthOfField;

            depthOfFieldValue.active = true;
            depthOfFieldValue.mode.overrideState = true;
            depthOfFieldValue.focalLength.overrideState = true;
            depthOfField.focusDistance.overrideState = true;
        }
        else
        {
            Debug.LogWarning("Depth of Field component not found in the assigned Volume Profile.");
        }

    }

    void Update()
    {

        RaycastForCalculation();

        /*
        depthOfFieldValue.focalLength.value = focalLength;
        depthOfFieldValue.focusDistance.value = focusDistance;
        */
    }

    void RaycastForCalculation()
    {

        RaycastHit hit;

        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, Mathf.Infinity, layerMask))
        {
            //Update the focus distance based on the raycast hit
            depthOfFieldValue.focusDistance.value = hit.distance;

            //Calculate the focal length based on the distance to the hit point
            float hitDistance = hit.distance;
            float focalLength = CalculateFocalLength(hitDistance);

            //Update the focal length
            depthOfFieldValue.focalLength.value = focalLength;
        }
    }

    float CalculateFocalLength(float distance)
    {
        // You can implement your own formula for calculating the focal length based on the distance
        // For example, you could use a linear or exponential relationship between distance and focal length
        // Here's a simple linear example:
        float minFocalLength = 0.1f; // Minimum focal length
        float maxFocalLength = 100f; // Maximum focal length
        float maxDistance = 100f; // Distance at which focal length becomes maximum
        float focalLength = Mathf.Lerp(minFocalLength, maxFocalLength, distance / maxDistance);
        return focalLength;
        
        /*
        // For this example, we will just return a constant value for demonstration
        return 30f; // Adjust this value according to your preference
        */
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(Camera.main.transform.position, Camera.main.transform.position + Camera.main.transform.forward * maxRaycastDistance);
    }

}
