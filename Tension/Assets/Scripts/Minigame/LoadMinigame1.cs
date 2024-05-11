using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMinigame1 : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        SceneManager.LoadSceneAsync("minigame", LoadSceneMode.Additive);
    }
}
