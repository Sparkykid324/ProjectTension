using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 14;

    void OnTriggerEnter(Collider other)
{
    if (other.gameObject.CompareTag("Player"))
    {
        other.gameObject.GetComponent<PlayerShooting>().ammoPool += ammoAmount;
        //destroy self
        Destroy(gameObject);
    }
}
}
