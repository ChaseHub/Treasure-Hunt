using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
public class ThirdPersonMovement : MonoBehaviour
{

    private CharacterController controller;
    private Transform cameraMainTransform;

    private float gravity = -9.81f;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private bool jumpPressed;
    
    [SerializeField] private float playerSpeed = 1.0f;
    [SerializeField] private float jumpMultiplier = 1.0f;
    [SerializeField] private float rotationSpeed = 4.0f;
    [SerializeField] private InputActionReference movementControl;
    [SerializeField] private InputActionReference jumpControl;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        cameraMainTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = movementControl.action.ReadValue<Vector2>();

        // Rotation
        if (movement != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + cameraMainTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }

        // Movement
        Vector3 move = new Vector3(movement.x, 0f, movement.y);
        move = cameraMainTransform.forward * move.z + cameraMainTransform.right * move.x;
        move.y = 0f;
        move *= playerSpeed;

        // Jumping
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer)
        {
            playerVelocity.y = 0.0f;

            if (jumpPressed)
            {
                playerVelocity.y += Mathf.Sqrt(jumpMultiplier * -1.0f * gravity);
                jumpPressed = false;
            }
        }
        playerVelocity.y += gravity * Time.deltaTime;

        // Move Controller
        controller.Move((playerVelocity + move) * Time.deltaTime);
    }

    private void OnJump()
    {
        if(controller.velocity.y == 0)
        {
            jumpPressed = true;    
        }
    }
}
