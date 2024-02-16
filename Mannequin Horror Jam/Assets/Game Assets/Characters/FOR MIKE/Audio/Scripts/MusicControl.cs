using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControl : MonoBehaviour
{

    BasicInventory inventory;

    void Start()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.sarahRoomMusic, this.transform.position);

        

    }

}
