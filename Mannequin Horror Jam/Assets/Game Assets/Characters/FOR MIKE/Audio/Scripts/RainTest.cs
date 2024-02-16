using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class RainTest : MonoBehaviour
{

    FMODEvents fmodEvents;

    void Start()
    {

        AudioManager.instance.PlayOneShot(FMODEvents.instance.rainAmbiance, this.transform.position);

    }

}
