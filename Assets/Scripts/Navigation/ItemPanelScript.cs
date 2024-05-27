using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemPanelScript : MonoBehaviour, ICancelHandler, ISubmitHandler
{
    public void OnCancel(BaseEventData eventData)
    {
        if (!GameLogic.instance.Player.GetComponent<ThirdPersonController>()._playerInput.currentControlScheme.Equals("KeyboardMouse")) { 
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void OnSubmit(BaseEventData eventData)
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
