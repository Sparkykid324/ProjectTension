using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeGameMaster : MonoBehaviour
{

    void Update()
    {
        if(AllPipesAreRight())
        {
            Debug.Log("All pipes are right");
        }
    }

    bool AllPipesAreRight()
    {
        // Checks if the pipes are all right, and returns true if they are

        foreach (GameObject pipe in GameObject.FindGameObjectsWithTag("ImportantPipe"))
        {
            if (!pipe.GetComponent<RotateMe>().isRight)
            {
                return false;
            }
        }

        return true;
    }


}
