using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeClamp : MonoBehaviour
{
    public float minRotation = -60f;
    public float maxRotation = 30f;

    void Update()
    {
        Vector3 currentRotation = transform.eulerAngles;

        float clampedRotationX = Mathf.Clamp(currentRotation.x, minRotation, maxRotation);

        transform.eulerAngles = new Vector3(clampedRotationX, currentRotation.y, currentRotation.z);
    }
}
