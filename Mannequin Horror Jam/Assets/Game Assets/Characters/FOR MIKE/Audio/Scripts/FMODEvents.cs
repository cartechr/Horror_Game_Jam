using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.Playables;
using FMOD.Studio;
using FMOD;

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
    [field: SerializeField] public EventReference doorCreak { get; private set; }
    public EventInstance doorCreakInst;
    [field: SerializeField] public EventReference doorRattle { get; private set; }
    public EventInstance doorRattleInst;
    [field: SerializeField] public EventReference doorUnlock { get; private set; }
    public EventInstance doorUnlockInst;
    [field: SerializeField] public EventReference floorCreak {  get; private set; }
    public EventInstance floorCreakInst;
    [field: SerializeField] public EventReference metalDoorClose { get; private set; }
    public EventInstance metalDoorCloseInst;
    [field: SerializeField] public EventReference metalDoorOpen { get; private set; }
    public EventInstance metalDoorOpenInst;


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

    //public FMOD.Studio.System.create 

    //EmitterGameEvent sarahRoomTrigger;

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
            UnityEngine.Debug.LogError(" Found more than one FMOD Events instance in the scene");
        }
        instance = this;

        //Music = FMODUnity.RuntimeManager.GetBus("bus:/Master/Music");
        //SFX = FMODUnity.RuntimeManager.GetBus("bus:/Master/SFX");
        //Master = FMODUnity.RuntimeManager.GetBus("bus:/Master");

    }

    private void Start()
    {
        //startSarah();
        //startHallWay(gameObject);
    }

    private void Update()
    {
        /*
        Music.setVolume(MusicVolume);
        SFX.setVolume(SFXVolume);
        Master.setVolume(MasterVolume);
        */
        if (Input.GetKeyDown(KeyCode.L))
        {
            changeHallway();
            //changeSarah();
            UnityEngine.Debug.Log("change hallway music");
        }



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


    #region Music

    //-------------------------------------------SARAH ROOM--------------------------------------------------------------
    public void startSarah()
    {
        sarahRoomMusicInst = FMODUnity.RuntimeManager.CreateInstance(sarahRoomMusic);
        sarahRoomMusicInst.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        sarahRoomMusicInst.start();
        UnityEngine.Debug.Log(sarahRoomMusicInst.ToString());
        //sarahRoomMusicInst.release();
    }

    public void changeSarah()
    {
        sarahRoomMusicInst.setParameterByNameWithLabel("PickupObject", "True", true);
    }
    public void stopSarah()
    {
        sarahRoomMusicInst.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    //-------------------------------------------HALLWAY MUSIC--------------------------------------------------------------

    public void startHallWay(GameObject gameobject)
    {
        hallwayMusicInst = FMODUnity.RuntimeManager.CreateInstance(hallwayMusic);
        hallwayMusicInst.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameobject));
        hallwayMusicInst.start();
        //hallwayMusicInst.release();
    }

    public void changeHallway()
    {
        //hallwayMusicInst.setParameterByName("Danger", 1, true);
        //FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Danger", 1, true);
        FMOD.Studio.System.create(out FMOD.Studio.System system);
        system.setParameterByName("Danger", 1, true);
    }
    #endregion

    #region Ambiance

    #endregion

}