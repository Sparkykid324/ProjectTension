using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    public GameObject MiniPlayer;

    private void OnTriggerEnter(Collider other)
    {
        MiniPlayer.gameObject.SetActive(false);
        MinigameChecker.Minigame2Complete = true;

        SceneManager.UnloadSceneAsync("minigame2");
    }
}
