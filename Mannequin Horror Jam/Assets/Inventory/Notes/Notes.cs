using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;




public class Notes : MonoBehaviour
{
    public Inventory_Manager inventoryManager;
    bool NoteNearby;
    bool ReadNote;

    [Header("GameObjects")]
    public GameObject Background;
    public GameObject ActiveNoteText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (inventoryManager.ActiveNoteNum)
        {
            case 0:
                ActiveNoteText.GetComponent<TextMeshProUGUI>().SetText("");
                break;
            case 1:
                ActiveNoteText.GetComponent<TextMeshProUGUI>().SetText("Note 1");
                break;
            case 2:
                ActiveNoteText.GetComponent<TextMeshProUGUI>().SetText("Note 2");
                break;
            case 3:
                ActiveNoteText.GetComponent<TextMeshProUGUI>().SetText("Note 3");
                break;
        }


        if (NoteNearby)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {

                int value = int.Parse(this.gameObject.name);
                inventoryManager.ActiveNoteNum = value;
                Debug.Log("Reading Note #" + value);

                ActiveNoteText.SetActive(true);
                Background.SetActive(true);
            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                ReadNote = true;
            }
        }
        if (ReadNote)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Debug.Log("Note Read");

                ActiveNoteText.SetActive(false);
                Background.SetActive(false);
                this.gameObject.SetActive(false);

                ReadNote = false;
                NoteNearby = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player nearby a note");
            NoteNearby = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            NoteNearby = false;
            Debug.Log("Player walked away from note");
        }
    }
}
