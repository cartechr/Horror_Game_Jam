using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public struct SubtitleText
{
    public float time;
    public string text;
}
public class Commentary : MonoBehaviour
{
    public SubtitleText[] Dialogue1;

    public GameObject subtitleGO;
    TextMeshProUGUI subtitles;


    float startClock = 0;
    float Clock;
    bool clockOver = true;

    bool isSpeaking = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        if (subtitles == null)
        {
            //Debug.Log("Returning Null");
            subtitles = subtitleGO.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            //Debug.Log("got subtitles");
        }
    }
    public void WakeUp()
    {
        isSpeaking = true;

        Debug.Log("Start Text");

        StartCoroutine(Subtitle());
    }

    IEnumerator Subtitle()
    {
        subtitleGO.SetActive(true);
        foreach (var line in Dialogue1)
        {
           
            subtitleGO.GetComponent<TextMeshProUGUI>().text = line.text;
 

            yield return new WaitForSeconds(line.time);
        }

        subtitleGO.SetActive(false);
    }
}
