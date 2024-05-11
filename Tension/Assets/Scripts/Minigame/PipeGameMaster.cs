using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PipeGameMaster : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        if (AllPipesAreRight())
        {
            //Debug.Log("All pipes are right");
            MinigameChecker.Minigame1Complete = true;

            SceneManager.UnloadSceneAsync("minigame2");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.UnloadSceneAsync("minigame");
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
