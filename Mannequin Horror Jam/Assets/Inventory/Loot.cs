using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Progress;

public class Loot : MonoBehaviour
{
    public Items item;

    public void Initialize(Items item)
    {
        this.item = item;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player detected");
            if (Input.GetKey(KeyCode.E))
            {
                Inventory_Manager.Instance.AddItem(item);
                this.gameObject.SetActive(false);
                Debug.Log(this.name + " has been collected");
            }
        }
    }
}