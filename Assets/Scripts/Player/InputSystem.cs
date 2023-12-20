/*******************************************************************************
Author: Jared
State: Incomplete
Description:
 Player movement functions and ground check.
 Ground check needs to be reworked to a collision counter.
*******************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystem : MonoBehaviour
{
    private Rigidbody Rigidbody;
    private PlayerInput playerInput;
    private PlayerActions playerActions;
    public float velocidad = 30f;
    public float jump = 10f;
    public bool grounded = false;
    RaycastHit hit;

    void Start()
    {
        
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, -Vector3.up);
        Debug.DrawRay(transform.position, Vector3.down * 10, Color.red);

        float height_above_ground = hit.distance;
        //Debug.Log(height_above_ground);

        if (Physics.Raycast(ray, out hit))
        {
            if (height_above_ground <= 1.0001f)
            {
                grounded = true;
            }
            else
            {
                grounded = false;
            }
        }
    }

    private void FixedUpdate()
    {
        //Definitely didn't just copy and paste this shit
        Vector2 inputVector = playerActions.PlayerControlsController.Movement.ReadValue<Vector2>();
        float speed = velocidad;

        // Get the player's current rotation
        Quaternion playerRotation = transform.rotation;

        // Calculate the forward direction relative to the player's rotation
        Vector3 forwardDirection = playerRotation * Vector3.forward;

        // Calculate the right direction relative to the player's rotation
        Vector3 rightDirection = playerRotation * Vector3.right;

        // Calculate the movement direction based on input
        Vector3 movement = (forwardDirection * inputVector.y) + (rightDirection * inputVector.x);

        // Check if the movement vector has a magnitude
        if (movement.magnitude > 0)
        {
            // Apply force in the movement direction
            Vector3 force = movement.normalized * speed;

            // Apply the force to the Rigidbody
            Rigidbody.AddForce(force, ForceMode.Force);
        }
    }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        
        playerActions = new PlayerActions();
        playerActions.PlayerControlsController.Enable();
        playerActions.PlayerControlsController.Jump.performed += Jump;
    }
    
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (grounded)
            {
                //Debug.Log("Jump " + context.phase);
                Rigidbody.AddForce(Vector3.up * jump, ForceMode.Impulse);
            }
        }
    }
}
