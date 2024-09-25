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
    private void Awake()
    {
        Oscie.sendDialogText += setDialog;
    }
    void Start()
    {
        textbox.text = "";
        gameObject.SetActive(false);
        initSpeed = speed;
        initWaitTime = waitTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            speed = 0.0000000001f;
            waitTime = 0f;
        }
        else
        {
            speed=initSpeed;
            waitTime=initWaitTime;
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
            transform.localScale = Vector3.one*(0.75f);
            foreach (char c in text[i].ToCharArray())
            {
                textbox.text += c;
                yield return new WaitForSeconds(speed);
                typeChar();
            }
            yield return new WaitForSeconds(waitTime);
            transform.localScale = Vector3.zero;
            textbox.text = "";
        }
        isTalking=false;
    }
}
