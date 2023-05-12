using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationFollower : MonoBehaviour
{
    public Transform Target;
    private float minRotation = 300f;
    private float maxRotation = 30f;
    

    void Update()
    {
        Vector3 currentRotation = Target.transform.eulerAngles;

        if(currentRotation.x >= 300 || currentRotation.x <= 30 || currentRotation.x == 0)
        {
            transform.rotation = Target.rotation;
        }
    }
}
