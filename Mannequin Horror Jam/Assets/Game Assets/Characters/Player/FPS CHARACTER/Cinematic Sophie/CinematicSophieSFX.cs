using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicSophieSFX : MonoBehaviour
{

    private void Start()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.sarahRoomMusic, this.transform.position);
    }


    public void SarahClothes()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.clothesRustling, this.transform.position);
    }


}
