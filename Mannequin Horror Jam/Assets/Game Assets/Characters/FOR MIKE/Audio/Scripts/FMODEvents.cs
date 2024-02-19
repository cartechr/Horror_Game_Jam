using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.Playables;
using FMOD.Studio;
using FMOD;
using UnityEngine.SceneManagement;

public class FMODEvents : MonoBehaviour
{
    //-----------------------------------------------EventReferences + EventInstances---------------------------------------------------
    #region Music
    [field: Header("Music")]
    [field: SerializeField] public EventReference sarahRoomMusic { get; private set; }
    public EventInstance sarahRoomMusicInst;
    [field: SerializeField] public EventReference hallwayMusic { get; private set; }
    public EventInstance hallwayMusicInst;

    #endregion

    #region Ambiance
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

    #endregion

    #region SFX
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

    #endregion

    #region Dialogue
    [field: Header("Dialogue")]

    [field: SerializeField] public EventReference gasping { get; private set; }
    public EventInstance gaspingInst;
    [field: SerializeField] public EventReference sprint { get; private set; }
    public EventInstance sprintInst;
    [field: SerializeField] public EventReference choking { get; private set; }
    public EventInstance chokingInst;

    [field: SerializeField] public EventReference Eating { get; private set; }
    public EventInstance EatingInst;

    //---------------------------------------------------------------------------

    [field: SerializeField] public EventReference YouInThere { get; private set; }
    public EventInstance YouInThereInst;

    [field: SerializeField] public EventReference FinallyAwake { get; private set; }
    public EventInstance FinallyAwakeInst;

    [field: SerializeField] public EventReference YouReading { get; private set; }
    public EventInstance YouReadingInst;

    [field: SerializeField] public EventReference someone { get; private set; }
    public EventInstance SomeoneInst;

    [field: SerializeField] public EventReference traitors { get; private set; }
    public EventInstance TraitorsInst;

    [field: SerializeField] public EventReference SafeToTalk { get; private set; }
    public EventInstance SafeToTalkInst;

    [field: SerializeField] public EventReference TheyDontBelieve { get; private set; }
    public EventInstance TheyDontBelieveInst;

    [field: SerializeField] public EventReference HungerDriveYou { get; private set; }
    public EventInstance HungerDriveYouInst;

    [field: SerializeField] public EventReference ChewCrunch { get; private set; }
    public EventInstance ChewCrunchInst;

    [field: SerializeField] public EventReference YouDontRecognise { get; private set; }
    public EventInstance YouDontRecogniseInst;

    [field: SerializeField] public EventReference EatSarah { get; private set; }
    public EventInstance EatSarahInst;

    [field: SerializeField] public EventReference freedom { get; private set; }
    public EventInstance FreedomInst;

    [field: SerializeField] public EventReference LibertaliaForever { get; private set; }
    public EventInstance LibertaliaForeverInst;
    #endregion

    #region Menu
    [field: SerializeField] public EventReference buttonPress { get; private set; }
    public EventInstance buttonPressInst;

    [field: SerializeField] public EventReference gameStart { get; private set; }
    public EventInstance gameStartInst;

    [field: SerializeField] public EventReference menuOpen { get; private set; }
    public EventInstance menuOpenInst;

    #endregion

    //-----------------------------------------------------------------------------------------------------------------------------------
    // Music Buses
    public Bus Music;
    public Bus SFX;
    public Bus Dialogue;

    public bool Options;

   public static FMODEvents instance { get; private set; }


    [Header("Global Parameters")]

    [Range(0, 10)]
    public float Promixity;

    bool isChasing;

    [Range(0, 5)]
    public float MasterVolume;

    [Range(0, 5)]
    public float MusicVolume;

    [Range(0, 5)]
    public float SFXVolume;

    [Range(0, 5)]
    public float DialogueVolume;

    /* private FMOD.Studio.Bus[] myBuses = new FMOD.Studio.Bus[12];
     private string busesList;
     private string busPath;
     private FMOD.Studio.Bank myBank;
     private int busCount;

     private string BusPath;
     public FMOD.RESULT busListOk;
     public FMOD.RESULT sysemIsOk;*/

    GameObject Manager;

    void Start()
    {
        /*  FMODUnity.RuntimeManager.StudioSystem.getBankList(out FMOD.Studio.Bank[] loadedBanks);
          foreach (FMOD.Studio.Bank bank in loadedBanks)
          {
              bank.getPath(out string path);
              busListOk = bank.getBusList(out myBuses);
              bank.getBusCount(out busCount);
              if (busCount > 0)
              {
                  foreach (var bus in myBuses)
                  {
                      bus.getPath(out busPath);
                     // UnityEngine.Debug.Log(busPath);
                  }
              }
          }*/

        MasterVolume = 5;
        MusicVolume = 5;
        SFXVolume = 5;
        DialogueVolume = 5;


    }
    private void Awake()
    {
        if(instance != null)
        {

        }
        instance = this;


        Music = FMODUnity.RuntimeManager.GetBus("bus:/Music");
        SFX = FMODUnity.RuntimeManager.GetBus("bus:/SFX");
        Dialogue = FMODUnity.RuntimeManager.GetBus("bus:/Dialogue");

    }


    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            Manager = GameObject.FindGameObjectWithTag("Manager");
            DontDestroyOnLoad(Manager);
        }

        if (Options)
        {
            Music.setVolume(MusicVolume);
            SFX.setVolume(SFXVolume);
            Dialogue.setVolume(DialogueVolume);
        }
    }
// -----------------------------------------------SOUND USE------------------------------------------------------------------
    #region SOUND CONTROL

    public void pauseFMOD()
    {
        Music.setPaused(true);
        SFX.setPaused(true);
        Dialogue.setPaused(true);
    }

    public void unpauseFMOD()
    {
        Music.setPaused(false);
        SFX.setPaused(false);
        Dialogue.setPaused(false);
    }

    #endregion
    #region Music

//------------------------------------------------SARAH ROOM--------------------------------------------------------------
    public void startSarah()
    {
        sarahRoomMusicInst = FMODUnity.RuntimeManager.CreateInstance(sarahRoomMusic);
        sarahRoomMusicInst.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        sarahRoomMusicInst.start();
        //sarahRoomMusicInst.release();
    }

    public void changeSarah()
    {
        sarahRoomMusicInst.setParameterByNameWithLabel("PickupObject", "True", true);
    }
    public void stopImmediateSarah()
    {
        sarahRoomMusicInst.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        sarahRoomMusicInst.release();
    }
    public void stopFadeSarah()
    {
        sarahRoomMusicInst.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        sarahRoomMusicInst.release();
    }

//-----------------------------------------------HALLWAY MUSIC--------------------------------------------------------------

    public void startHallWay(GameObject gameobject)
    {
        hallwayMusicInst = FMODUnity.RuntimeManager.CreateInstance(hallwayMusic);
        hallwayMusicInst.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameobject));
        hallwayMusicInst.start();
        //hallwayMusicInst.release();
    }

    public void dangerHallway() 
    { 
        hallwayMusicInst.setParameterByName("Danger", 1, true);

        isChasing = true;
    }

    public void peaceHallway()
    {
        hallwayMusicInst.setParameterByName("Danger", 0, true);

        isChasing = false;
    }

    public void stopHallway()
    {
        hallwayMusicInst.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        hallwayMusicInst.release();
    }
    #endregion

    #region Dialogue


    public void InThere(GameObject gameobject)
    {
        YouInThereInst = FMODUnity.RuntimeManager.CreateInstance(YouInThere);
        YouInThereInst.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameobject));
        YouInThereInst.start();
        YouInThereInst.release();
    }

    public void finAwake(GameObject gameobject)
    {
        FinallyAwakeInst = FMODUnity.RuntimeManager.CreateInstance(FinallyAwake);
        FinallyAwakeInst.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameobject));
        FinallyAwakeInst.start();
        FinallyAwakeInst.release();
    }

    public void Reading(GameObject gameobject)
    {
        YouReadingInst = FMODUnity.RuntimeManager.CreateInstance(YouReading);
        YouReadingInst.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameobject));
        YouReadingInst.start();
        YouReadingInst.release();
    }

    public void Someone(GameObject gameobject)
    {
        SomeoneInst = FMODUnity.RuntimeManager.CreateInstance(someone);
        SomeoneInst.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameobject));
        SomeoneInst.start();
        SomeoneInst.release();
    }

    public void Traitors(GameObject gameobject)
    {
        TraitorsInst = FMODUnity.RuntimeManager.CreateInstance(traitors);
        TraitorsInst.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameobject));
        TraitorsInst.start();
        TraitorsInst.release();
    }

    public void Talk(GameObject gameobject)
    {
        SafeToTalkInst = FMODUnity.RuntimeManager.CreateInstance(SafeToTalk);
        SafeToTalkInst.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameobject));
        SafeToTalkInst.start();
        SafeToTalkInst.release();
    }

    public void DontBelieve(GameObject gameobject)
    {
        TheyDontBelieveInst = FMODUnity.RuntimeManager.CreateInstance(TheyDontBelieve);
        TheyDontBelieveInst.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameobject));
        TheyDontBelieveInst.start();
        TheyDontBelieveInst.release();
    }

    public void Hunger(GameObject gameobject)
    {
        HungerDriveYouInst = FMODUnity.RuntimeManager.CreateInstance(HungerDriveYou);
        HungerDriveYouInst.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameobject));
        HungerDriveYouInst.start();
        HungerDriveYouInst.release();
    }

    public void Crunch(GameObject gameobject)
    {
        ChewCrunchInst = FMODUnity.RuntimeManager.CreateInstance(ChewCrunch);
        ChewCrunchInst.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameobject));
        ChewCrunchInst.start();
        ChewCrunchInst.release();
    }

    public void Recognise(GameObject gameobject)
    {
        YouDontRecogniseInst = FMODUnity.RuntimeManager.CreateInstance(YouDontRecognise);
        YouDontRecogniseInst.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameobject));
        YouDontRecogniseInst.start();
        YouDontRecogniseInst.release();
    }

    public void eatSarah(GameObject gameobject)
    {
        EatSarahInst = FMODUnity.RuntimeManager.CreateInstance(EatSarah);
        EatSarahInst.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameobject));
        EatSarahInst.start();
        EatSarahInst.release();
    }

    public void Freedom(GameObject gameobject)
    {
        FreedomInst = FMODUnity.RuntimeManager.CreateInstance(freedom);
        FreedomInst.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameobject));
        FreedomInst.start();
        FreedomInst.release();
    }

    public void Forever(GameObject gameobject)
    {
        LibertaliaForeverInst = FMODUnity.RuntimeManager.CreateInstance(LibertaliaForever);
        LibertaliaForeverInst.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameobject));
        LibertaliaForeverInst.start();
        LibertaliaForeverInst.release();
    }

    #endregion

    #region Item & Interaction

    public void startRattle(GameObject gameobject)
    {
        doorCreakInst = FMODUnity.RuntimeManager.CreateInstance(doorRattle);
        doorCreakInst.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameobject));
        doorCreakInst.start();
        doorCreakInst.release();
    }

    public void stopRattle()
    {

    }

    public void doorUnluck(GameObject gameobject)
    {
        doorUnlockInst = FMODUnity.RuntimeManager.CreateInstance(doorUnlock);
        doorUnlockInst.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameobject));
        doorUnlockInst.start();
        doorUnlockInst.release();
    }

    #endregion

    #region AI

    public void startMannequinMove(GameObject gameobject)
    {
        mannequinMovementInst = FMODUnity.RuntimeManager.CreateInstance(mannequinMovement);
        mannequinMovementInst.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameobject));
        mannequinMovementInst.start();
        //mannequinMovementInst.release();
    }

    public void stopMannequinMove()
    {
        mannequinMovementInst.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        mannequinMovementInst.release();
    }
    public void startGasping(GameObject gameobject)
    {
        gaspingInst = FMODUnity.RuntimeManager.CreateInstance(gasping);
        gaspingInst.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameobject));
        gaspingInst.start();
    }

    public void stopGasping()
    {
        gaspingInst.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        gaspingInst.release();
    }

    public void startChoking(GameObject gameobject)
    {
        chokingInst = FMODUnity.RuntimeManager.CreateInstance(choking);
        chokingInst.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameobject));
        chokingInst.start();
    }

    public void stopChoking()
    {
        chokingInst.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        chokingInst.release();
    }

    public void changeToFullHealth(GameObject gameobject)
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("FullHealth", 0, true);

        takingDamageInst = FMODUnity.RuntimeManager.CreateInstance(takingDamage);
        takingDamageInst.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameobject));
        takingDamageInst.start();
        takingDamageInst.release();
    }

    public void changeToHealth(GameObject gameobject)
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("FullHealth", 1, true);

        takingDamageInst = FMODUnity.RuntimeManager.CreateInstance(takingDamage);
        takingDamageInst.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameobject));
        takingDamageInst.start();
        takingDamageInst.release();
    }

    #endregion

    public void startRain(GameObject gameobject)
    {
        rainAmbianceInst = FMODUnity.RuntimeManager.CreateInstance(rainAmbiance);
        rainAmbianceInst.start();
    }

    public void stopRain()
    {
        rainAmbianceInst.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
    public void playRain()
    {
        rainAmbianceInst.start();
    }

    public void endRain()
    {
        rainAmbianceInst.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        rainAmbianceInst.release();
    }
    public void pressButton()
    {
        buttonPressInst = FMODUnity.RuntimeManager.CreateInstance(buttonPress);
        buttonPressInst.start();
        buttonPressInst.release();
    }

    public void startGame()
    {
        gameStartInst = FMODUnity.RuntimeManager.CreateInstance(gameStart);
        gameStartInst.start();
        gameStartInst.release();
    }

    public void openMenu()
    {
        menuOpenInst = FMODUnity.RuntimeManager.CreateInstance(menuOpen);
        menuOpenInst.start();
    }
}