using StarterAssets;
using UnityEngine;
using UnityEngine.EventSystems;

public class SellButton : MonoBehaviour , ISubmitHandler
{

    public GameObject SlotAbove;
    public void OnSubmit(BaseEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(SlotAbove);
    }
}
