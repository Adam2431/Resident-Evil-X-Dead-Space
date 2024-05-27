using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public GameObject player;
    public GameObject KeyCanvas;
    public GameObject Inventory;

    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < 2)
        {
            KeyCanvas.SetActive(true);
            if (player.GetComponent<ThirdPersonController>()._input.interact)
            {
                if(Inventory.GetComponent<Inventory>().items.Count < 6) { 
                    player.GetComponent<ThirdPersonController>()._input.interact = false;
                    Inventory.GetComponent<Inventory>().AddToInventory(Inventory.GetComponent<Inventory>().scriptableObjects[24]);
                    KeyCanvas.SetActive(false);
                    Destroy(gameObject);
                }
                else
                {
                    //Play Error Sound
                }
            }

        }
        else
        {
            KeyCanvas.SetActive(false);
        }
    }
}
