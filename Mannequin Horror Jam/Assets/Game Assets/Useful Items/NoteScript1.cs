using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteScript1 : MonoBehaviour
{
    Animator animator;
    GameObject Player;

    private void Start()
    {
        animator = GetComponent<Animator>();

    }

    private void Update()
    {
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    public void stopAnimation()
    {
        animator.SetBool("Note", false);
    }

}
