using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetector : MonoBehaviour
{
    public GameObject Player;
    private void OnTriggerEnter(Collider other)
    {
        Player.gameObject.SetActive(false);
        Player.gameObject.transform.position = new Vector3(2499.7f, 5.0799f, 40.67f);
        Player.gameObject.SetActive(true);
    }
}
