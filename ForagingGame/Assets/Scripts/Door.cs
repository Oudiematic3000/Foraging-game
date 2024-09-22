using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    
    public void changeScene()
    {
        if (SceneManager.GetActiveScene().name == "Cottage")
        {
            SceneManager.LoadScene("Prototype");
        }
        else
        {
            SceneManager.LoadScene("Cottage");
        }
    }
   
}
