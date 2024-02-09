using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitch : MonoBehaviour
{

    public GameObject player;

    void SwitchToPlayer()
    {
        player.SetActive(true);
        this.gameObject.SetActive(false);


    }


}
