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
        for (int i = 0; i < text.Length; i++)
        {
            transform.localScale = Vector3.one*(0.75f);
            foreach (char c in text[i].ToCharArray())
            {
                textbox.text += c;
                yield return new WaitForSeconds(speed);
                typeChar();
            }
            yield return new WaitForSeconds(1.5f);
            transform.localScale = Vector3.zero;
            textbox.text = "";
        }
    }
}
