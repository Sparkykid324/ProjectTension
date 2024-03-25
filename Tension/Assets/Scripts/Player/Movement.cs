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
    [SerializeField] float velocityY;
    [SerializeField] bool isGrounded;

    bool isSprinting = false;
    float originalSpeed;
    float sprintSpeed;

    [SerializeField] Transform crouchGroundCheck;
    [SerializeField] bool isCrouchingCheck = false;
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
        isCrouchingCheck = Physics.CheckSphere(crouchGroundCheck.position, 0.5f, ground); //checks empty to see if player should be crouching (used to prevent crouching whilst player is airborne)

        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        //velocityY -= gravity * -2.0f * Time.deltaTime; //PART OF OLD BROKEN JUMP - constantly added negative y velocity (Removing this broke crouch)

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * Speed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);

        /*if (isGrounded && Input.GetButtonDown("Jump")) PART OF OLD BROKEN JUMP
        {
            velocityY = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (!isGrounded && controller.velocity.y < -1f)
        {
            velocityY = -8f;
        } PART OF OLD BROKEN JUMP*/

        //Jump
        if (!isCrouching && controller.velocity.y < -0.1f && isGrounded)
        {
            velocityY = 0f; //resets y velocity when touching the ground (Seems to be working intermittently)
            //Debug.Log("Y Velocity has been Reset.");
        }

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocityY = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (!isGrounded /*&& controller.velocity.y < 0.1f*/)
        {
            velocityY -= gravity * -2.0f * Time.deltaTime;
            //Debug.Log("going down");
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
        if (Input.GetKey(KeyCode.LeftControl) && isGrounded && !isCrouchingCheck)
        {
            isSprinting = false;
            isCrouching = true;
        }
        else if (!Input.GetKey(KeyCode.LeftControl) || !isCrouchingCheck)
        {
            isCrouching = false;
        }

        if (isCrouching == true)
        {
            velocityY = -3.0f; //adds gravity when crouching in order for the capsule to actually move down despite being grounded
            controller.height = crouchingHeight;
            Speed = crouchSpeed;
            //isGrounded = false;
        }
        else
        {
            controller.height = standingHeight;
        }
    }
}
