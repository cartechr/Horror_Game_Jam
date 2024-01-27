using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{

    [field: Header("SFX")]

    [field: SerializeField] public EventReference sarahRoom { get; private set; }
    [field: SerializeField] public EventReference windowRain { get; private set; }
    [field: SerializeField] public EventReference footStep { get; private set; }

    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError(" Found more than one FMOD Events instance in the scene");
        }
        instance = this;
    }

    private void Start()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.sarahRoom, this.transform.position);
    }

    private void Update()
    {
        
    }


}
