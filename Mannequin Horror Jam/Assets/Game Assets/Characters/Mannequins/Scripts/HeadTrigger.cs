using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using FMODUnity;

public class HeadTrigger : MonoBehaviour
{

    [Header("Assignments")]
    [Tooltip("True or False for Mannequin to turn head")]
    [SerializeField] bool headShouldTurn;
    [Tooltip("FOR DEBUG DO NOT TOUCH")]
    [SerializeField] bool headTrigger;
    [Tooltip("Assign Mannequins")]
    [SerializeField] GameObject[] mannequins;
    [Tooltip("Float variable for head turn DO NOT CHANGE")]
    [SerializeField] float weight;
    [Tooltip("Weight Increase Duration or how slow should the head turn?")]
    [SerializeField] float increaseDuration = 3f;
    [Tooltip("Assign Collider Here")]
    [SerializeField] Collider triggerCollider;
    [Tooltip("This will be pulled automatically")]
    [SerializeField] MultiAimConstraint multiAimConstraint;

    [SerializeField] EventReference headTurnSound;

    void Start()
    {
        //Get the components in all mannequins
        foreach (GameObject mannequin in mannequins)
        {
            multiAimConstraint = mannequin.GetComponentInChildren<MultiAimConstraint>();
            //audioSource = mannequin.GetComponent<AudioSource>();
            if(multiAimConstraint != null)
            {
                multiAimConstraint.weight = weight;
            }
            else
            {
                Debug.LogWarning("MultiAimConstraint not found on the specified mannequin: " + mannequin.name);
            }
        }


    }

    public void OnTriggerEnter(Collider other)
    {
        if(headShouldTurn == true)
        {
            

            //Update the weight for all mannequins
            foreach (GameObject mannequin in mannequins)
            {
                multiAimConstraint = mannequin.GetComponentInChildren<MultiAimConstraint>();

                if (multiAimConstraint != null)
                {
                    AudioManager.instance.PlayOneShot(headTurnSound, this.transform.position);
                    StartCoroutine(IncreaseWeightGradually(multiAimConstraint, weight + 1f, increaseDuration));
                    Debug.Log("Weight is increasing");

                    headTrigger = true;
                    Debug.Log("headTrigger = " + headTrigger);

                }
                else
                {
                    Debug.Log("MultiAim Constraint is null");
                }

                

            }
        }

        if (headShouldTurn == false) 
        {
            

            //Update the weight for all mannequins
            foreach (GameObject mannequin in mannequins)
            {
                multiAimConstraint = mannequin.GetComponentInChildren<MultiAimConstraint>();

                if (multiAimConstraint != null)
                {
                    //audioSource.Play();
                    StartCoroutine(IncreaseWeightGradually(multiAimConstraint, weight - 1f, increaseDuration));
                    Debug.Log("Head is not turning anymore");

                    headTrigger = false;
                    Debug.Log("headTrigger = " + headTrigger);

                }
                else
                {
                    Debug.Log("MultiAim Constraint is null");
                }

            }
              
        } 
        
    }

    IEnumerator IncreaseWeightGradually(MultiAimConstraint constraint, float targetWeight, float duration)
    {
        float initialWeight = constraint.weight;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            
            constraint.weight = Mathf.Lerp(initialWeight, targetWeight, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        //Ensure the final weight is set exactly
        constraint.weight = targetWeight;

    }
   
    


}
