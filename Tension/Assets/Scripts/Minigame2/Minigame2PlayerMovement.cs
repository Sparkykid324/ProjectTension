using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Minigame2PlayerMovement : MonoBehaviour
{
    [SerializeField] bool cursorLock = true;
    [SerializeField] float Speed = 40.0f;
    [SerializeField][Range(0.0f, 0.5f)] float moveSmoothTime = 0.05f;
    [SerializeField] float gravity = -30f;

    float velocityY;

    CharacterController controller;
    Vector2 currentDir;
    Vector2 currentDirVelocity;
    Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        UpdateMove();

        /*if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.UnloadSceneAsync("minigame2");
        }*/
    }

    void UpdateMove()
    {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        velocityY -= gravity * -2.0f * Time.deltaTime;

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * Speed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);
    }
}