using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscie : MonoBehaviour
{
    public GameObject[] tools;
    public static event Action<string[]> sendDialogText;
    public string[] osciePickupDialogue, notebookFirstTimeDialog;
    public string[] spokenDialogue;
    public string[] testDialog;
    private void Awake()
    {
        Dialogue.typeChar += speak;
        FirstPersonControls.pickedUp += getDialog;
    }
    private void OnDestroy()
    {
        Dialogue.typeChar -= speak;
        FirstPersonControls.pickedUp -= getDialog;
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
    public void getDialog(GameObject g)
    {
        if (g.GetComponent<Oscie>())
        {
            sendDialogText(osciePickupDialogue);
        }else if (g.GetComponent<InventoryManager>())
        {
            GetComponent<AudioSource>().spatialBlend = 0;
            sendDialogText(notebookFirstTimeDialog);
        }
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
