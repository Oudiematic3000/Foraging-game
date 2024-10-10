using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Feedback : MonoBehaviour
{
    public TextMeshProUGUI feedbackText;
    public Transform elementSlots;
    void Awake()
    {
        CookBookManager.SendFeedback += DisplayFeedback;
    }

    // Update is called once per frame
    void Update()
    {
        if(FindAnyObjectByType<CookBookManager>().isOpen)
        {
            transform.localScale = Vector3.zero;
        }
    }

    public void toggle()
    {
        if(transform.localScale == Vector3.zero)
        {
            transform.localScale = Vector3.one;
        }
        else
        {
            transform.localScale = Vector3.zero;
        }
    }

    public void DisplayFeedback()
    {

        string output = "";
        
        foreach (Transform eSlot in elementSlots)
        {
            ElementSlot e = eSlot.GetComponent<ElementSlot>();
            output += "<size=150%>" + e.ElementTitle.text + "<size=100%>\n";
            if (e.missFlavs.Count <= 2)
            {
                foreach (string miss in e.missFlavs)
                {
                    output += "Needs something " +miss +"...\n";
                }
            }
            else
            {
                output += "Needs more flavour...\n";
            }

            foreach(string wrong in e.wrongFlavs)
            {
                output += "Too " + wrong + "!\n";
            }
        }
        feedbackText.text = output;
    }
}
