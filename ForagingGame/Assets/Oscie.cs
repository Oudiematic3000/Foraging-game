using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscie : MonoBehaviour
{
    public GameObject[] tools;
    
    void Start()
    {
        foreach (GameObject go in tools)
        {
            go.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showTool(int i)
    {
        foreach(GameObject go in tools)
        {
            go.SetActive(false);
        }
        if (i != 0)
        {
            tools[i].SetActive(true);
        }
    }
}
