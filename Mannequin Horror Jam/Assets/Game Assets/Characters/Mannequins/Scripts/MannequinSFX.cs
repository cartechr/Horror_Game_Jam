using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MannequinSFX : MonoBehaviour
{

    public void PlayMannequinFootstep()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.mannequinMovement, this.transform.position);
    }

    public void PlayMannequinMaskRattle()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.maskRattle, this.transform.position);
    }

    public void PlayMannequinHeadTurn()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.headTurns, this.transform.position);
    }


}
