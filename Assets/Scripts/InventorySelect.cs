using StarterAssets;
using UnityEngine;

public class InventorySelect : MonoBehaviour
{
    public RectTransform menu;
    [SerializeField] private GameObject Player;

    // Update is called once per frame
    void Update()
    {
        if (Player.GetComponent<ThirdPersonController>()._input.select)
        {
            menu.gameObject.SetActive(true);
            Vector2 menuPosition = Input.mousePosition;
            menu.position = (Vector3)menuPosition;
        }
    }

}
