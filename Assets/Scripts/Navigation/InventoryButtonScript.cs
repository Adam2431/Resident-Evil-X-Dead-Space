using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryButtonScript : MonoBehaviour, IMoveHandler, ISubmitHandler
{
    [SerializeField] private GameObject RightGameObject;
    [SerializeField] private GameObject LeftGameObject;
    [SerializeField] private GameObject UpGameObject;
    [SerializeField] private GameObject UpGameObject2;
    [SerializeField] private GameObject DownGameObject;
    [SerializeField] private GameObject DownGameObject2;

    public void OnMove(AxisEventData eventData)
    {
        Vector2 moveVector = eventData.moveVector;

        if (moveVector.x > 0.5f)
        {
            if(RightGameObject.activeSelf)
                EventSystem.current.SetSelectedGameObject(RightGameObject);
        }
        else if (moveVector.x < -0.5f)
        {
            if(LeftGameObject.activeSelf)
                EventSystem.current.SetSelectedGameObject(LeftGameObject);
        }
        else if (moveVector.y < -0.5f)
        {
            if (DownGameObject.activeSelf)
                EventSystem.current.SetSelectedGameObject(DownGameObject);
            else if (DownGameObject2.activeSelf)
                EventSystem.current.SetSelectedGameObject(DownGameObject2);
        }
        else if (moveVector.y > 0.5f)
        {
            if (UpGameObject.activeSelf)
                EventSystem.current.SetSelectedGameObject(UpGameObject);
            else if (UpGameObject2.activeSelf)
                EventSystem.current.SetSelectedGameObject(UpGameObject2);
        }
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (LeftGameObject.activeSelf)
            EventSystem.current.SetSelectedGameObject(LeftGameObject);
        else if (RightGameObject.activeSelf)
            EventSystem.current.SetSelectedGameObject(RightGameObject);
        else if (UpGameObject.activeSelf)
            EventSystem.current.SetSelectedGameObject(UpGameObject);
        else if (UpGameObject2.activeSelf)
            EventSystem.current.SetSelectedGameObject(UpGameObject2);
        else if (DownGameObject.activeSelf)
            EventSystem.current.SetSelectedGameObject(DownGameObject);
        else if (DownGameObject2.activeSelf)
            EventSystem.current.SetSelectedGameObject(DownGameObject2);
    }
}
