using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]

public class ThirdPersonMovement : MonoBehaviour
{

    private CharacterController controller;
    private Vector2 input;
    private Vector3 direction;
    private float currentVelocity;
    private float gravity = -9.81f;
    private float velocity;
    
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float smoothTime = 0.05f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    [SerializeField] private float jumpMultiplier = 1.0f;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        ApplyGravity();
        ApplyRotation();
        ApplyMovement();
        
    }

    private void ApplyGravity()
    {
        if(controller.isGrounded && velocity < 0.0f)
        {
            velocity = -1.0f;
        } else
        {
            velocity += gravity * gravityMultiplier * Time.deltaTime;
        }
        direction.y = velocity;
    }

    private void ApplyRotation()
    {
        if (input.sqrMagnitude == 0) return;

        var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime);

        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }

    private void ApplyMovement()
    {
        controller.Move(direction * speed * Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
        direction = new Vector3(input.x, 0.0f, input.y);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        if (!controller.isGrounded)
        {
            return;
        }
        velocity += jumpMultiplier;
    }
}
