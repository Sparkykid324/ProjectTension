using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using TMPro;


public class PlayerShooting : MonoBehaviour
{
    //Player Shooting/Looking
    public Camera playerCamera;
    public LayerMask enemyLayer;
     public float shootingRange = 100f;

     //Player Reloading
    public float ammoPool = 21f;
    public int currentBullets;
    private int ammoNeeded;
    public int maxBullets = 7;

    //Display
    public TMP_Text ammoDisplay;

    //Animation
    public Animator gunAnimator;
    public string animationClipName = "Shoot";
    public string reloadClipName = "Reload";

    private void Start()
    {
        currentBullets = maxBullets; //Sets the current bullets to the max bullets
        Cursor.lockState = CursorLockMode.Locked; //Locks the cursor to the center of the screen
        ammoDisplay = GameObject.Find("AmmoDisplay").GetComponent<TMP_Text>(); //Finds the ammo display
        ammoDisplay.text = currentBullets + " / " + ammoPool; //Displays the current bullets and ammo pool

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && currentBullets > 0 && Time.timeScale == 1.0f) // Shoots the gun if the player clicks and there is ammo in the gun
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R) && currentBullets < maxBullets) // Reloads the gun if the player presses R and there is not a full clip in the gun
        {
            Reload();
        }
        ammoDisplay.text = currentBullets + " / " + ammoPool; //Displays the current bullets and ammo pool

        // PlayerLooking(); // Calls the PlayerLooking function
    }

    private void Shoot() // Shoots the gun by raycasting 
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        gunAnimator.SetTrigger(animationClipName); //Plays the shooting animation

        if (Physics.Raycast(ray, out hit, shootingRange, enemyLayer))
        {
            // Get the EnemyHealth component from the hit game object
            EnemyChasePlayer enemyHealth = hit.collider.GetComponent<EnemyChasePlayer>();

            if (enemyHealth != null) // If the enemyHealth is not null, then we hit an enemy
            {
                // Send a message to the enemy to take damage
                enemyHealth.TakeDamage();
            }
        }

        // Decrease the bullet count
        currentBullets--;
    }

    private void Reload()
    {
        
        if(ammoPool > 0) //Reloads the gun if there is ammo in the pool
        {
            ammoNeeded = maxBullets - currentBullets; //Figures out how much ammo it needs to reload 
            if(ammoPool >= ammoNeeded) // If there is enough ammo in the pool to reload the gun, it will reload the gun and subtract the ammo from the pool
            {

                gunAnimator.SetTrigger(reloadClipName); //Plays the reload animation

                currentBullets = maxBullets;
                ammoPool -= ammoNeeded;
            }
            else //If there is not enough ammo in the pool to reload the gun, it will just add the remaining ammo to the gun
            {
                currentBullets += (int)ammoPool;
                ammoPool = 0;
            }
        }
        else //No ammo in the pool
        {
            Debug.Log("No Ammo");
        }
    }

    // public void PlayerLooking()
    // {   
    //     //Player Looking
    //     float mouseX = Input.GetAxis("Mouse X");
    //     float mouseY = Input.GetAxis("Mouse Y");

    //     //Player Rotation
    //     Vector3 currentRotation = transform.localEulerAngles;
    //     currentRotation.y += mouseX;
    //     transform.localRotation = Quaternion.AngleAxis(currentRotation.y, Vector3.up);

    //     //Camera Rotation
    //     Vector3 currentCameraRotation = playerCamera.transform.localEulerAngles;
    //     currentCameraRotation.x -= mouseY;
    //     playerCamera.transform.localRotation = Quaternion.AngleAxis(currentCameraRotation.x, Vector3.right);
    // }

    public void AmmoRefill() //Refills the ammo pool
    {
        ammoPool =+ 7f;

    }
}
