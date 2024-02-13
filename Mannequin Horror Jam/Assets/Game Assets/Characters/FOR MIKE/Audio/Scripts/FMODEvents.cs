using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.Playables;

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
    [field: SerializeField] public EventReference keyPickup { get; private set; }
    [field: SerializeField] public EventReference notePickup { get; private set; }


    [field: Header("Footstep Specific")]
    [field: SerializeField] public EventReference footStep { get; private set; }

    [field: Header("Clothes SFX")]
    [field: SerializeField] public EventReference clothesRustling { get; private set; }

    [field: Header("Mannequins")]
    [field: SerializeField] public EventReference headTurns { get; private set; }
    [field: SerializeField] public EventReference mannequinMovement { get; private set; }
    [field: SerializeField] public EventReference maskRattle { get; private set; }

    [field: Header("Flashlight")]
    [field: SerializeField] public EventReference flashlightClick { get; private set; }
    [field: SerializeField] public EventReference flashlightPickup { get; private set; }

    [field: Header("Walkie-Talkie")]
    [field: SerializeField] public EventReference walkiePickup { get; private set; }

    [field: Header("Damage Indicator")]
    [field: SerializeField] public EventReference takingDamage { get; private set; }

    EmitterGameEvent sarahRoomTrigger;

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

    public void SetMusicVolume()
    {
        
    }

    public void PlayHallwayMusic()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.sarahRoomMusic, this.transform.position);
    }

    public void PlaySarahRoomMusic()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.hallwayMusic, this.transform.position);
    }

    public void PlayRainAmbiance()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.rainAmbiance, this.transform.position);
    }

    public void PlayStairWellAmbiance()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.stairWellAmbiance, this.transform.position);
    }

    public void PlayerNoteInteraction()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.notePickup, this.transform.position);
    }

    public void SarahRoommMusicChange()
    {
        //FMOD.Studio.PARAMETER_ID id = //HOW TO REACH TO THE PARAMETER PROPERT?!?!

    }

}
