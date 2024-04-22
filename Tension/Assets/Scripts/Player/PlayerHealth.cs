using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 3.0f;
    public float currentHealth = 3.0f;
    public GameObject bloodEffect;

    // Start is called before the first frame update

    void Start()
    {
        bloodEffect = GameObject.Find("BloodOver");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Image bloodImage = bloodEffect.GetComponent<Image>(); // Assign the Image component to a variable
        Color bloodColor = bloodImage.color; // Get the color from the Image component
        bloodColor.a -= 0.02f; // Modify the alpha value of the color
        bloodImage.color = bloodColor; // Assign the modified color back to the Image component
    }
    
    public void TakeDamage()
    {
        currentHealth -= 1f;
        Image bloodImage = bloodEffect.GetComponent<Image>(); // Assign the Image component to a variable
        Color bloodColor = bloodImage.color; // Get the color from the Image component
        bloodColor.a = 0.52f; // Modify the alpha value of the color
        bloodImage.color = bloodColor; // Assign the modified color back to the Image component

        if (currentHealth <= 0f)
        {
            // Implement when player dies and we have menu
            SceneManager.LoadScene("MainMenu");




            Debug.Log("Dead");
        }
    }

}
