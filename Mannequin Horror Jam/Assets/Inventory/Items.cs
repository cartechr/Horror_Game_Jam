using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Items : ScriptableObject
{

    [Header("Only gameplay")]
    public ItemType type;
    public ActionType actionType;

    [Header("Only UI")]
    public Sprite image;


    public enum ItemType
    {
        Flashlight,
        Pipe,
        Key
    }

    public enum ActionType
    {
        Shine,
        Defend,
        Unlock
    }
}