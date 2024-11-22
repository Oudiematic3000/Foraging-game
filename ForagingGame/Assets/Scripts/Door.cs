using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public int currentScene;

    public void changeScene()
    {
        if (currentScene == 0){
            
            SceneManager.LoadSceneAsync("Cottage", LoadSceneMode.Additive);
            SceneManager.LoadScene("Player+UI", LoadSceneMode.Additive);
            StartCoroutine(wait());
            
            

        }
        if (currentScene==3)
        {
            SceneManager.LoadSceneAsync("Prototype", LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync("Cottage");
        }
        if (currentScene==4)
        {
            SceneManager.LoadSceneAsync("Cottage", LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync("Prototype");
        }
    }
    public IEnumerator wait()
    {
        yield return new WaitForSeconds(0.2f);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Player+UI"));
        SceneManager.UnloadSceneAsync("MainMenu");
    }
}

