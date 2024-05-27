using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScript : MonoBehaviour
{
    void Update()
    {
        Vector3 currentRotation = transform.localEulerAngles;
        currentRotation.y += 6.0f * 15f * Time.unscaledDeltaTime;
        transform.localEulerAngles = currentRotation;
    }
}
