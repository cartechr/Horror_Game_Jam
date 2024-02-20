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
    public SubtitleText[] Dialogue2;
    public GameObject subtitleGO;
    TextMeshProUGUI subtitles;

    public bool isTalking;

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
    public void StartDialogue(SubtitleText[] subtitle)
    {
        Debug.Log("Start Text");

        StartCoroutine(Subtitle(subtitle));
    }

    IEnumerator Subtitle(SubtitleText[] subtitle)
    {
        isTalking = true;
        subtitleGO.SetActive(true);
        foreach (var line in subtitle)
        {
           
            subtitleGO.GetComponent<TextMeshProUGUI>().text = line.text;
 

            yield return new WaitForSeconds(line.time);
        }
        isTalking = false;
        subtitleGO.SetActive(false);
    }
}
