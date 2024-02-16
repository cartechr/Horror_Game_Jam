using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.Playables;
using FMOD.Studio;

public class FMODEvents : MonoBehaviour
{

    [field: Header("Music")]
    [field: SerializeField] public EventReference sarahRoomMusic { get; private set; }
    public EventInstance sarahRoomMusicInst;
    [field: SerializeField] public EventReference hallwayMusic { get; private set; }
    public EventInstance hallwayMusicInst;


    [field: Header("Ambiance")]
    [field: SerializeField] public EventReference stairWellAmbiance { get; private set; }
    public EventInstance stairWellAmbianceInst;
    [field: SerializeField] public EventReference rainAmbiance { get; private set; }
    public EventInstance rainAmbianceInst;


    [field: Header("Item & Interaction")]
    [field: SerializeField] public EventReference uiInteractSFX { get; private set; }
    public EventInstance uiInteractSFXInst;
    [field: SerializeField] public EventReference keyPickup { get; private set; }
    public EventInstance keyPickupInst;
    [field: SerializeField] public EventReference notePickup { get; private set; }
    public EventInstance notePickupInst;


    [field: Header("Footstep Specific")]
    [field: SerializeField] public EventReference footStep { get; private set; }
    public EventInstance footStepInst;

    [field: Header("Clothes SFX")]
    [field: SerializeField] public EventReference clothesRustling { get; private set; }
    public EventInstance clothesRustlingInst;

    [field: Header("Mannequins")]
    [field: SerializeField] public EventReference headTurns { get; private set; }
    public EventInstance headTurnInst;
    [field: SerializeField] public EventReference mannequinMovement { get; private set; }
    public EventInstance mannequinMovementInst;
    [field: SerializeField] public EventReference maskRattle { get; private set; }
    public EventInstance maskRattleInst;

    [field: Header("Flashlight")]
    [field: SerializeField] public EventReference flashlightClick { get; private set; }
    public EventInstance flashlightClickInst;
    [field: SerializeField] public EventReference flashlightPickup { get; private set; }
    public EventInstance flashlightPickupInst;

    [field: Header("Walkie-Talkie")]
    [field: SerializeField] public EventReference walkiePickup { get; private set; }
    public EventInstance walkiePickupInst;

    [field: Header("Damage Indicator")]
    [field: SerializeField] public EventReference takingDamage { get; private set; }
    public EventInstance takingDamageInst;



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


    public void parameterChange(EventInstance eventInstance, string parameterName, string label)
    {
        eventInstance.setParameterByNameWithLabel(parameterName, label);
    }


    public void startFX(EventInstance eventInstance, EventReference eventReference)
    {
        eventInstance = FMODUnity.RuntimeManager.CreateInstance(eventReference);
        eventInstance.start();
    }

    public void stopImmediateFX(EventInstance eventInstance, EventReference eventReference)
    {
        eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        eventInstance.release();
    }

    public void stopFadeFX(EventInstance eventInstance, EventReference eventReference)
    {
        eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        eventInstance.release();
    }
}
