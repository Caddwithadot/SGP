using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class Test : MonoBehaviour
{
    private Gamepad gamepad;

    private void Start()
    {
        // Check if there's a gamepad connected
        if (Gamepad.all.Count > 0)
        {
            gamepad = Gamepad.all[0];
        }
    }

    private void Update()
    {
        // Check if the gamepad is connected and print its state
        if (gamepad != null)
        {
            Debug.Log("Gamepad connected!");
            Debug.Log($"LeftStick: {gamepad.leftStick.ReadValue()} RightStick: {gamepad.rightStick.ReadValue()}");

            // You can also use gamepad.buttonSouth.wasPressedThisFrame to check if the A button was pressed this frame
        }
        else
        {
            Debug.Log("Gamepad not connected!");
        }

        // Check if the current platform is mobile and do something
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Debug.Log("Running on mobile device!");
            // Do something for mobile
        }
        else
        {
            Debug.Log("Running on desktop!");
            // Do something for desktop
        }
    }
}