using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBodyToCamera : MonoBehaviour
{

    [SerializeField] Transform cameraPosition;
    [SerializeField] float rotationSpeed = 5f;

    void Update()
    {
        RotateCharacter();
    }

    void RotateCharacter()
    {
        // Get the camera's forward direction without considering the y-axis rotation
        Vector3 forwardDirection = cameraPosition.forward;
        forwardDirection.y = 0f;
        forwardDirection.Normalize();

        // Calculate the rotation angle based on the camera's forward direction
        float targetAngle = Mathf.Atan2(forwardDirection.x, forwardDirection.z) * Mathf.Rad2Deg;

        // Smoothly interpolate the character's rotation towards the target angle
        float smoothedAngle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, rotationSpeed * Time.deltaTime);

        // Apply the new rotation to the character
        transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);
    }
}
