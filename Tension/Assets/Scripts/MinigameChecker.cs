using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameChecker : MonoBehaviour
{
    public GameObject Player;
    public GameObject Ammo;
    public GameObject Crosshair;
    public GameObject PauseMenu;
    
    public static bool Minigame1Complete = true;
    public static bool Minigame2Complete = false;

    public void Update()
    {
        if(SceneManager.sceneCount > 1)
        {
            InMinigame();
        }

        if (SceneManager.sceneCount == 1)
        {
            NotInMinigame();
        }
    }
    public void InMinigame()
    {
        Player.gameObject.SetActive(false);
        Ammo.gameObject.SetActive(false);
        Crosshair.gameObject.SetActive(false);
        PauseMenu.gameObject.SetActive(false);
    }

    public void NotInMinigame()
    {
        Player.gameObject.SetActive(true);
        Ammo.gameObject.SetActive(true);
        Crosshair.gameObject.SetActive(true);
        PauseMenu.gameObject.SetActive(true);
    }
}
