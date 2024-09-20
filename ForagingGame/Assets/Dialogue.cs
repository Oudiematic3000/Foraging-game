using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textbox;
    public string[] text;
    public float speed;
    private int index;
    public AudioManager audioManager;
    void Start()
    {
        textbox.text = "";
        startDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void startDialogue()
    {
        index=0;
        StartCoroutine(typeLine());
    }
    
    public IEnumerator typeLine()
    {
        foreach(char c in text[index].ToCharArray())
        {
            textbox.text += c;
            yield return new WaitForSeconds(speed);
            audioManager.sounds[1].source.Play();
        }
    }
}
