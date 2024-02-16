using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicSophieSFX : MonoBehaviour
{

    private void Start()
    {

    }


    public void SarahClothes()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.clothesRustling, this.transform.position);
    }


}
