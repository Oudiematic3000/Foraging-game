using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscie : MonoBehaviour
{
    public GameObject[] tools;

    private void Awake()
    {
        Dialogue.typeChar += speak;
    }
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

    public void speak()
    {
        GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);
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
