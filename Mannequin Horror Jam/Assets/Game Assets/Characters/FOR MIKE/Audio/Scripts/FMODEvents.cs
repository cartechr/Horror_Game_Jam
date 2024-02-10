using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{

    [field: Header("Music")]
    [field: SerializeField] public EventReference sarahRoomMusic { get; private set; }
    [field: SerializeField] public EventReference hallwayMusic { get; private set; }


    [field: Header("Ambiance")]
    [field: SerializeField] public EventReference stairWellAmbiance { get; private set; }
    [field: SerializeField] public EventReference rainAmbiance { get; private set; }


    [field: Header("Item & Interaction")]
    [field: SerializeField] public EventReference uiInteractSFX { get; private set; }


    [field: Header("Footstep Specific")]
    [field: SerializeField] public EventReference footStep { get; private set; }


    public static FMODEvents instance { get; private set; }
    

    FMOD.Studio.Bus Music;
    FMOD.Studio.Bus SFX;
    FMOD.Studio.Bus Master;

    float MusicVolume = 0.5f;
    float SFXVolume = 0.5f;
    float MasterVolume = 1f;

    public float musicVolume = 1;


    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError(" Found more than one FMOD Events instance in the scene");
        }
        instance = this;

        //Music = FMODUnity.RuntimeManager.GetBus("bus:/Master/Music");
        //SFX = FMODUnity.RuntimeManager.GetBus("bus:/Master/SFX");
        //Master = FMODUnity.RuntimeManager.GetBus("bus:/Master");

    }

    private void Start()
    {
        //PlayMusic();
        
    }

    private void Update()
    {
        /*
        Music.setVolume(MusicVolume);
        SFX.setVolume(SFXVolume);
        Master.setVolume(MasterVolume);
        */
    }

    /*
    public void MasterVolumeLevel(float newMasterVolume)
    {
        MasterVolume = newMasterVolume;
    }

    public void MusicVolumeLevel(float newMusicVolume)
    {
        MusicVolume = newMusicVolume;
    }

    public void SFXVolumeLevel(float newSFXVolume)
    {
        SFXVolume = newSFXVolume;
    }
    */

    void SetMusicVolume()
    {
        
    }

    void PlayHallwayMusic()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.sarahRoomMusic, this.transform.position);
    }

    void PlaySarahRoomMusic()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.hallwayMusic, this.transform.position);
    }

    void PlayRainAmbiance()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.rainAmbiance, this.transform.position);
    }

    void PlayStairWellAmbiance()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.stairWellAmbiance, this.transform.position);
    }

}
