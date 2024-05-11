using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMinigame2 : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        SceneManager.LoadSceneAsync("minigame2", LoadSceneMode.Additive);
    }
}
