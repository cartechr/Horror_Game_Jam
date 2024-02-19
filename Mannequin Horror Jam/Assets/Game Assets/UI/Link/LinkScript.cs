using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class LinkScript : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public string websiteURL = "https://www.youtube.com/watch?v=a25YllpL90Y&ab_channel=Arkhangelbeats";
    private bool isPointerDown = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isPointerDown)
        {
            Application.OpenURL(websiteURL);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
    }
}
