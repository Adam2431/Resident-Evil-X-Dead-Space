using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse3D : MonoBehaviour
{

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            transform.position = hit.point;
        }
    }
}
