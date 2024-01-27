using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStabilizers : MonoBehaviour
{

    public Transform headTransform;

    private void LateUpdate()
    {
        this.transform.position = headTransform.position;
    }


}
