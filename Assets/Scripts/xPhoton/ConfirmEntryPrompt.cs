/*
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ConfirmEntryPrompt : MonoBehaviour
{
    public GameObject ConfirmCanvas;

    private void Update()
    {
        if(Variables.Object(this).Get<bool>("bool") == true)
        {
            OnConfirm();
            Variables.Object(this).Set("bool", false);
        }
    }

    public void OnConfirm()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Destroy(ConfirmCanvas);
    }
}
*/