using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseBackCancelScript : MonoBehaviour, ICancelHandler
{
    public void OnCancel(BaseEventData eventData)
    {
        GameLogic.instance.Back();
    }
}
