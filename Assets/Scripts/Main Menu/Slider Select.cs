using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderSelect : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private Text Text;
    [SerializeField] private GameObject Audio;
    public void OnSelect(BaseEventData eventData)
    {
        Text.color = new Color(1, 1, 1, 1);
        Audio.GetComponent<Audio>().PlayHoverSound();
    }
    public void OnDeselect(BaseEventData eventData)
    {
        Text.color = new Color(0.55f, 0.55f, 0.55f, 1);
    }
}
