using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 3.0f;
    public float currentHealth = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void TakeDamage()
    {
        currentHealth -= 1f;
        if (currentHealth <= 0f)
        {
            // Implement when player dies and we have menu
            SceneManager.LoadScene("MainMenu");




            Debug.Log("Dead");
        }
    }

}
