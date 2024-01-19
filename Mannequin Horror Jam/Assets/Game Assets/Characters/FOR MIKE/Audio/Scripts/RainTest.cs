using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class RainTest : MonoBehaviour
{

    public void OnTriggerEnter(Collider other)
    {

        AudioManager.instance.PlayOneShot(FMODEvents.instance.windowRain, this.transform.position);


    }

}
