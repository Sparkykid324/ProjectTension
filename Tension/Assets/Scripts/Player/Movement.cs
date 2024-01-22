using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] Transform playerCamera;
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;
    [SerializeField] bool cursorLock = true;
    [SerializeField] public float mouseSensitivity = 3.5f;
    [SerializeField] float Speed = 6.0f;
    [SerializeField][Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
    [SerializeField] float gravity = -30f;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;

    public float jumpHeight = 6f;
    float velocityY;
    bool isGrounded;

    bool isSprinting = false;
    float originalSpeed;
    float sprintSpeed;

    bool isCrouching = false;
    float standingHeight;
    float crouchingHeight = 1.25f;
    float crouchSpeed;

    float cameraCap;
    Vector2 currentMouseDelta;
    Vector2 currentMouseDeltaVelocity;

    CharacterController controller;
    Vector2 currentDir;
    Vector2 currentDirVelocity;
    Vector3 velocity;

    //variable to record whether game is paused or not to prevent camera movement (currently not working)
    public bool pauseLock = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
        }

        originalSpeed = Speed;
        sprintSpeed = Speed + 4f;

        crouchSpeed = Speed - 4f;
        standingHeight = controller.height;
    }

    void Update()
    {
        UpdateMouse();
        UpdateMove();
    }

    void UpdateMouse()
    {
        if (pauseLock == false)
        {
            Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

            cameraCap -= currentMouseDelta.y * mouseSensitivity;

            cameraCap = Mathf.Clamp(cameraCap, -90.0f, 90.0f);

            playerCamera.localEulerAngles = Vector3.right * cameraCap;

            transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);

            //Debug.Log("pauselock is false");
        }
        else
        {
            //Debug.Log("pauselock is true");
        }
    }

    void UpdateMove()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, ground);

        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        velocityY -= gravity * -2.0f * Time.deltaTime;

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * Speed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocityY = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (!isGrounded && controller.velocity.y < -1f)
        {
            velocityY = -8f;
        }

        //Sprint
        if (Input.GetKey(KeyCode.LeftShift) && isCrouching == false)
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }

        if (isSprinting == true)
        {
            Speed = sprintSpeed;
        }
        else
        {
            Speed = originalSpeed;
        }

        //Crouch
        if (Input.GetKey(KeyCode.LeftControl))
        {
            isSprinting = false;
            isCrouching = true;
        }
        else
        {
            isCrouching = false;
        }

        if (isCrouching == true)
        {
            controller.height = crouchingHeight;
            Speed = crouchSpeed;
            isGrounded = false;
        }
        else
        {
            controller.height = standingHeight;
        }
    }
}
