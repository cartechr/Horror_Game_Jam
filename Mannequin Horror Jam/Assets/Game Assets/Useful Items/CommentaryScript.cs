using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CommentaryScript : MonoBehaviour
{
    [Header("Dialogue Options")]
    [TextArea(3, 10)]
    [Tooltip("Enter your text here")]
    [SerializeField] string[] enterTexts;
    //[Tooltip("Decide how long should a text appear")]
    //[SerializeField] float waitPeriod;

    [Header("Assignments (Do it Manually")]
    [Tooltip("It is inside Player>Player UI Canvas>Commentary Panel")]
    [SerializeField] GameObject commentaryPanel;
    [Tooltip("It is inside Player>Player UI Canvas>Commentary Panel>Text Mesh Pro")]
    [SerializeField] TextMeshProUGUI commentaryText;

    [Header("Player Related Assignments")]
    [Tooltip("It will be assigned automatically")]
    [SerializeField] public FPSCONTROL fpsControl;
    [SerializeField] public GameObject player;

    public bool inDialogue;

    private void Start()
    {
        //player = GameObject.FindWithTag("Player");

        //fpsControl = GameObject.FindWithTag("Player").GetComponent<FPSCONTROL>();

    }

    private void Update()
    {

    }

    public void DialogueStarter()
    {
        StartCoroutine(StartDialogue());
    }

    public IEnumerator StartDialogue()
    {
        commentaryPanel.SetActive(true);
        fpsControl.animator.SetBool("isInteracting", true);

        for (int i = 0; i < enterTexts.Length; i++)
        {
            inDialogue = true;
            Debug.Log("inDialogue " + inDialogue);
            fpsControl.disableMovement = true;
            Debug.Log("Movement is disabled");
            commentaryText.text = enterTexts[i];
            //yield return new WaitForSeconds(waitPeriod);

            yield return new WaitForSeconds(0.5f);

            yield return WaitForInput(KeyCode.E);

        }

        inDialogue = false;
        Debug.Log("inDialogue " + inDialogue);
        fpsControl.disableMovement = false;
        Debug.Log("Movement is enabled");
        commentaryPanel.SetActive(false);
        fpsControl.animator.SetBool("isInteracting", false);
    }

    private IEnumerator WaitForInput(KeyCode keyCode)
    {
        // Wait until the specified key is pressed
        while (!Input.GetKeyDown(keyCode))
        {
            yield return null;
        }
    }

}
