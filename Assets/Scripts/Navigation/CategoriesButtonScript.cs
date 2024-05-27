using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class CategoriesButtonScript : MonoBehaviour, IMoveHandler
{

    [SerializeField] private GameObject RightGameObject;
    [SerializeField] private GameObject LeftGameObject;
    [SerializeField] private GameObject UpGameObject;
    [SerializeField] private GameObject DownGameObject1;
    [SerializeField] private GameObject DownGameObject2;
    [SerializeField] private GameObject DownGameObject3;

    [SerializeField] private GameObject DownGameObjectParent1;
    [SerializeField] private GameObject DownGameObjectParent2;
    [SerializeField] private GameObject DownGameObjectParent3;

    public void OnMove(AxisEventData eventData)
    {
        Vector2 moveVector = eventData.moveVector;

        if(moveVector.x > 0.5f)
        {
            EventSystem.current.SetSelectedGameObject(RightGameObject);
        }
        else if(moveVector.x < -0.5f)
        {
            EventSystem.current.SetSelectedGameObject(LeftGameObject);
        }
        else if(moveVector.y > 0.5f)
        {
            EventSystem.current.SetSelectedGameObject(UpGameObject);
        }
        else if(moveVector.y < -0.5f)
        {
            if(DownGameObjectParent1.activeSelf)
                EventSystem.current.SetSelectedGameObject(DownGameObject1);
            else if(DownGameObjectParent2.activeSelf)
                EventSystem.current.SetSelectedGameObject(DownGameObject2);
            else if(DownGameObjectParent3.activeSelf)
                EventSystem.current.SetSelectedGameObject(DownGameObject3);
        }
    }
}
