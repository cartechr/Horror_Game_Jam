using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanRotate : MonoBehaviour
{
    public float rotationSpeed = 50f; // Adjust the rotation speed as needed

    void Update()
    {
        // Rotate the object on the Y-axis
        float rotationAmount = rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, rotationAmount);

        // Alternatively, you can use the following line to rotate the object in fixed update:
        // transform.Rotate(Vector3.up, rotationSpeed * Time.fixedDeltaTime);
    }
}
