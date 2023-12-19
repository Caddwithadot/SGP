/*******************************************************************************
Author: Jared
State: Complete/Functional
Description:
 Clamps the player model's eyes between two angles.
*******************************************************************************/

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
        //Debug.Log(currentRotation.x);

        if (currentRotation.x >= minRotation || currentRotation.x <= maxRotation)
        {
            transform.rotation = Target.rotation;
        }
    }
}
