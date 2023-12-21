using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RotateMe : MonoBehaviour
{
    public GameObject[] pipes;
    public GameObject[] importantPipes;
    public Vector3[] correctRotation;
    public bool isRight = false;


    // Start is called before the first frame update
    void Start()
    {
        // Finds all the objects marked with the tag "Pipe" and "ImportantPipe", and stores them in the arrays pipes and importantPipes
        pipes = GameObject.FindGameObjectsWithTag("Pipe");
        importantPipes = GameObject.FindGameObjectsWithTag("ImportantPipe");
        isRight = false;
    }

    // Update is called once per frame
    void Update()
    {
        RotatePipes();
    }

    public void RotatePipes()
    {
        //when the player clicks the mouse over a pipe, rotate the pipe 90 degrees
        if (Input.GetMouseButtonDown(0))
        {
            //get the mouse position
            Vector3 mousePos = Input.mousePosition;

            Ray mouseRay = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hitInfo;

            if (Physics.Raycast(mouseRay, out hitInfo) && hitInfo.collider.gameObject.tag == "Pipe" || hitInfo.collider.gameObject.tag == "ImportantPipe")
            {
                GameObject pipe = hitInfo.collider.gameObject;

                // Calculate the new rotation, ensuring it stays within 0 to 360 degrees
                Vector3 newRotation = pipe.transform.eulerAngles - new Vector3(0, 90, 0);
                newRotation.y = Mathf.Repeat(newRotation.y, 360f);

                // Apply the new rotation
                pipe.transform.eulerAngles = newRotation;

            }
        }
    }

    void FixedUpdate()
    {
        // Checks if the pipes are in the correct rotation (correct rotation set in the inspector)
        foreach(Vector3 possibleAnswer in correctRotation){
            if (transform.eulerAngles.y == possibleAnswer.y && gameObject.tag == "ImportantPipe")
            {
                isRight = true;
                break;
            }
            else
            {
                isRight = false;
            }
        }
    }
}
