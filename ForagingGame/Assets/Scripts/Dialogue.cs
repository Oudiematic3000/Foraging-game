using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textbox;
    public string[] text;
    public float speed;
    public AudioManager audioManager;
    public static event Action typeChar;
    public bool isTalking=false;
    public float waitTime;
    private float initSpeed;
    private float initWaitTime;
    public bool skip =false;
    public bool finish =false;
    public bool typing = false;
    private void Awake()
    {
        Oscie.sendDialogText += setDialog;
    }
    void Start()
    {
        textbox.text = "";
        gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
        
            
           if(!typing && !finish) finish = true;
           if (typing && !finish) skip = true;

        }
        else
        {
            
            finish = false;
        }

        
    }

    public void setDialog(string[] arr)
    {
        text= arr;
        startDialogue();
    }

   public void startDialogue()
    {
        gameObject.SetActive(true);
       
        StartCoroutine(typeLine());
    }

    public IEnumerator typeLine()
    {
        isTalking = true;
        for (int i = 0; i < text.Length; i++)
        {
            finish = false;
            skip = false;
            transform.localScale = Vector3.one*(0.75f);
            
            typing = true;
            foreach (char c in text[i].ToCharArray())
            {
                if (!skip)
                {
                    textbox.text += c;
                    yield return new WaitForSeconds(speed);
                    typeChar();
                }
                else
                {
                    skip = false;
                    textbox.text = text[i];
                    
                    break;
                    
                }
            }
            typing = false;
            skip = false;
            yield return new WaitForSeconds(0.2f);
            yield return new WaitWhile(()=>!finish);
           
            transform.localScale = Vector3.zero;
            textbox.text = "";
        }
        isTalking=false;
    }
}
