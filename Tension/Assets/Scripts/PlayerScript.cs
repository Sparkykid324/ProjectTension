using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Constantly move self to mouse position
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 44;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = mouseWorldPosition;


    }
}
