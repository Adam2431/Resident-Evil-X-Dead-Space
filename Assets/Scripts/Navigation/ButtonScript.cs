using StarterAssets;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScript : MonoBehaviour , ISelectHandler , IPointerEnterHandler, ICancelHandler
{
    public void OnCancel(BaseEventData eventData)
    {
        if (!GameLogic.instance.Player.GetComponent<ThirdPersonController>()._playerInput.currentControlScheme.Equals("KeyboardMouse"))
        {
            GameLogic.instance.CloseStore();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameLogic.instance.AudioScript.PlayHoverSound();
    }

    public void OnSelect(BaseEventData eventData)
    {
        GameLogic.instance.AudioScript.PlayHoverSound();
    }
}
