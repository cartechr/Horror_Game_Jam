using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuSounds : MonoBehaviour
{
    public Slider music;
    public Slider sfx;
    public Slider dialogue;

    public Button Play;
    public Button Options;
    public Button Credits;
    public Button Exit;

    public GameObject fmodObject;
    public FMODEvents fmodEvents;

    // Start is called before the first frame update
    void Start()
    {
        fmodObject = GameObject.FindGameObjectWithTag("FMODEvents");
        fmodEvents = fmodObject.GetComponent<FMODEvents>();

        //fmodEvents.startGame();
    }

    // Update is called once per frame
    void Update()
    {
        fmodEvents.MusicVolume = music.value;
        fmodEvents.SFXVolume = sfx.value;
        fmodEvents.DialogueVolume = dialogue.value;
    }
}
