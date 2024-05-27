using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseCancelScript : MonoBehaviour , ICancelHandler
{
    public void OnCancel(BaseEventData eventData)
    {
        if(!GameLogic.instance.Player.GetComponent<ThirdPersonController>()._playerInput.currentControlScheme.Equals("KeyboardMouse"))
            GameLogic.instance.ResumeGame();
    }
}