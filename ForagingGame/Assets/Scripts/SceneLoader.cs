using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadSceneAsync("Prototype", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("Player+UI", LoadSceneMode.Additive);
    }

   
}
