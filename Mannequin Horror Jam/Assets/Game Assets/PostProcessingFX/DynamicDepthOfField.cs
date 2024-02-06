using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessing : MonoBehaviour
{

    public VolumeProfile volumeProfile;

    DepthOfField depthOfField;

    [Header("Focus Distance Settings")]
    public float maxFocusDistance;
    public float minFocusDistance;
    public float speedFocusChange;
    public float changeDepthOfField;

    [Header("Raycast Variables")]
    public float distanceBetween; //Global variable to store the Vector3.Distance variable to calculate distance between camera and raycast hit.distance

    public LayerMask layerMask;

    static float t = 0.0f; //starting value for the Lerp



    void Start()
    {

        if(volumeProfile.TryGet<DepthOfField>(out depthOfField))
        {
            Debug.Log("Depth of Field Component Initiated");
            depthOfField.active = true;
            depthOfField.mode.overrideState = true;
            depthOfField.focalLength.overrideState = true;
            depthOfField.focusDistance.overrideState = true;
        }
    }

    void Update()
    {
        RaycastHit hit;

        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Raycast hit: " + hit.transform.name);

            //Calculate the distance between camera and the raycast hit
            distanceBetween = Vector3.Distance(Camera.main.transform.position, hit.point);

            depthOfField.focusDistance.value = distanceBetween;

        }
    }

}
