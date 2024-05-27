using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryCancel : MonoBehaviour, ICancelHandler
{
    [SerializeField] private GameObject InventoryUI;
    [SerializeField] private List<GameObject> Slots;
    public void OnCancel(BaseEventData eventData)
    {
        if (!GameLogic.instance.Player.GetComponent<ThirdPersonController>()._playerInput.currentControlScheme.Equals("KeyboardMouse")) { 
            for (int i = 0; i < Slots.Count; i++)
            {
                if (Slots[i].activeSelf)
                {
                    Slots[i].SetActive(false);
                    return;
                }
            }
            InventoryUI.SetActive(false);
            GameLogic.instance.ResumeGame();
        }
    }
}
