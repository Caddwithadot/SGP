using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    float xRotation = 0f;
    public CinemachineFreeLook freeLookCamera;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        xRotation = 0f;
        freeLookCamera.m_XAxis.m_InputAxisName = "";
        freeLookCamera.m_YAxis.m_InputAxisName = "";
    }

    void Update()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue() * mouseSensitivity * Time.deltaTime;

        float mouseX = mouseDelta.x;
        float mouseY = mouseDelta.y;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        freeLookCamera.m_XAxis.Value += mouseX;
        freeLookCamera.m_YAxis.Value -= mouseY;
    }
}