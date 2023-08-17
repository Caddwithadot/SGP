using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmButton : MonoBehaviour
{
    public GameObject ConfirmCanvas;

    public void OnConfirmClick()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Destroy(ConfirmCanvas);
    }
}
