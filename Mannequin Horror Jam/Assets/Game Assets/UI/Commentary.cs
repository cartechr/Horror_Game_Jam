using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Commentary : MonoBehaviour
{

    public GameObject textMeshObject;
    TextMeshProUGUI textMeshProUGUI;

    int switchWake = 0;

    float startClock = 0;
    float Clock;
    bool clockOver = true;

    bool isSpeaking = false;


    // Start is called before the first frame update
    void Start()
    {
        textMeshProUGUI = textMeshObject.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        switch (switchWake)
        {
            case 1:
                textMeshProUGUI.text = "Hey you in there?";

                if (clockOver)
                {
                    Clock = 1.5f;
                    clockOver = false;
                }
                break;
            case 2:
                textMeshProUGUI.text = "Sarah get up";

                if (clockOver)
                {
                    Clock = 3f;
                    clockOver = false;
                }
                break;
            case 3:
                textMeshProUGUI.text = "Not gonna wait forever!";

                if (clockOver)
                {
                    Clock = 1.7f;
                    clockOver = false;
                }
                break;
            case 4:
                textMeshProUGUI.text = "Sarah?";

                if (clockOver)
                {
                    Clock = 2.4f;
                    clockOver = false;
                }
                break;
            case 5:
                textMeshProUGUI.text = "Hey!?";

                if (clockOver)
                {
                    Clock = 4f;
                    clockOver = false;
                }
                break;
            case 6:
                textMeshProUGUI.text = "Hey!";

                if (clockOver)
                {
                    Clock = 1.5f;
                    clockOver = false;
                }
                break;
            case 7:
                textMeshProUGUI.text = "Not in the mood for this again Sarah!";

                if (clockOver)
                {
                    Clock = 1.8f;
                    clockOver = false;
                }
                break;
            case 8:
                textMeshProUGUI.text = "";
                isSpeaking = false;
                break;
        }
        if (isSpeaking)
        {
            if (startClock < Clock)
            {
                startClock += Time.deltaTime;
               // textMeshProUGUI.text = "";
            }
            else
                {
                    startClock = 0;
                    clockOver = true;
                    switchWake += 1;
                    Debug.Log("Switch = " + switchWake);
                }
        }
    }
    public void WakeUp()
    {
        switchWake = 1;
        isSpeaking = true;

        textMeshObject.SetActive(true);
        Debug.Log("Start Text");
    }
}
