using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationFollower : MonoBehaviour
{
    public Transform Target;
    public float minRotation = 300f;
    public float maxRotation = 30f;

    void Update()
    {
        Vector3 currentRotation = Target.transform.eulerAngles;

        if(currentRotation.x >= minRotation || currentRotation.x <= maxRotation || currentRotation.x == 0)
        {
            transform.rotation = Target.rotation;
        }
        else if(currentRotation.x < minRotation && currentRotation.x >= 270)
        {
            transform.rotation = Quaternion.Euler(minRotation, currentRotation.y, currentRotation.z);
        }
        else if(currentRotation.x > maxRotation && currentRotation.x <= 90)
        {
            transform.rotation = Quaternion.Euler(maxRotation, currentRotation.y , currentRotation.z);
        }
    }
}
