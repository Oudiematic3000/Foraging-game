using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscie : MonoBehaviour
{
    public GameObject[] tools;
    public static event Action<string[]> sendDialogText;
    public string[] osciePickupDialogue, notebookFirstTimeDialogue, notebookFirstIngredientDialogue, holdItemFirstTimeDialogue, pickupToolFirstTimeDialogue;
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
        if (FindAnyObjectByType<FirstPersonControls>().holdingOscie) Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void getDialog(string s)
    {
        if (s=="PickupOscie")
        {
            sendDialogText(osciePickupDialogue);
        }else if (s=="OpenNotebookFirstTime")
        {
            GetComponent<AudioSource>().spatialBlend = 0;
            sendDialogText(notebookFirstTimeDialogue);
        }else if (s == "OpenNotebookFirstIngredient")
        {
            sendDialogText(notebookFirstIngredientDialogue);
        }else if (s == "HoldItemFirstTime")
        {
            sendDialogText(holdItemFirstTimeDialogue);
        }else if (s == "PickupToolFirstTime")
        {
            sendDialogText(pickupToolFirstTimeDialogue);
        }
    }
    public void speak()
    {
        GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);
        //GetComponent<AudioSource>().Play();
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
