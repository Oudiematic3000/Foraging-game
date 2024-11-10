using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public void closeGame()
    {
        Application.Quit();
    }

    public void openPauseMenu()
    {
        if (GameObject.Find("Pause").transform.localScale == Vector3.zero)
        {
            GameObject.Find("Pause").transform.localScale = Vector3.one;

        }
        else
        {
            GameObject.Find("Pause").transform.localScale = Vector3.zero;
        }
    }

    public void goMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
