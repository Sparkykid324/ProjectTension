using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 10f;
    public float currentHealth = 10f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage()
    {
        currentHealth -= 5f;
        if (currentHealth < 0)
        {
            Destroy(gameObject);
        }
    }
}
