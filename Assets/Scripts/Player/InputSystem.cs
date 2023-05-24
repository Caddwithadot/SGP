using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystem : MonoBehaviour
{
    private Rigidbody Rigidbody;
    private PlayerInput playerInput;
    private PlayerActions playerActions;
    public bool grounded = false;
    public float groundCheckDistance;
    private float bufferCheckDistance = -0.625f;
    float meshColliderHeight;

    void Start()
    {
        
    }

    void Update()
    {
        MeshCollider meshCollider = transform.GetChild(1).GetComponent<MeshCollider>();
        Bounds bounds = meshCollider.bounds;
        meshColliderHeight = bounds.size.y;
        groundCheckDistance = (meshColliderHeight / 2) + bufferCheckDistance;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, groundCheckDistance))
        {
            grounded = true;
            Debug.DrawRay(hit.point, hit.normal, Color.red);
        }
        else
        {
            grounded = false;
        }
    }

    private void FixedUpdate()
    {
        //Definitely didn't just copy and paste this shit
        Vector2 inputVector = playerActions.PlayerControlsController.Movement.ReadValue<Vector2>();
        float speed = 30f;

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
                Rigidbody.AddForce(Vector3.up * 5f, ForceMode.Impulse);
                grounded = false;
            }
        }
    }
}
