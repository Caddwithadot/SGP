using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceAlignment : MonoBehaviour
{
    [SerializeField]
    private LayerMask WhatIsGround;

    [SerializeField]
    private AnimationCurve animCurve;

    [SerializeField]
    private float Time;

    void Start()
    {
        
    }

    void Update()
    {
        SurfaceAlignmentFunction();
    }

    private void SurfaceAlignmentFunction()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit info = new RaycastHit();
        if (Physics.Raycast(ray, out info, WhatIsGround))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(Vector3.up, info.normal), animCurve.Evaluate(Time));
        }
    }
}
