using UnityEngine;
using UnityEngine.EventSystems;

public class SlotOptionsCancel : MonoBehaviour , ICancelHandler
{
    [SerializeField] private GameObject Slot;
    [SerializeField] private GameObject Parent;
    public void OnCancel(BaseEventData eventData)
    {
        Slot.SetActive(false);
        EventSystem.current.SetSelectedGameObject(Parent);
    }
}
