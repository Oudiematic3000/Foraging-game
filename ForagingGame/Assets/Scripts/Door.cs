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
        if (currentScene==0)
        {
            SceneManager.LoadSceneAsync("Prototype", LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync("Cottage");
        }
        if (currentScene==1)
        {
            SceneManager.LoadSceneAsync("Cottage", LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync("Prototype");
        }
    }
   
}
