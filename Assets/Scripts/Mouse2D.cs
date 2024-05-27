using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    void Update()
    {
        Vector3 mouseWorldPosition = Input.mousePosition;

        mouseWorldPosition.z = 0;
        Debug.Log(mouseWorldPosition);
        transform.position = mouseWorldPosition;
    }
}
