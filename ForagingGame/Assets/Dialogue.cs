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
    void Start()
    {
        textbox.text = "";
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
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

                audioManager.sounds[1].source.PlayOneShot(audioManager.sounds[1].clip);
            }
            yield return new WaitForSeconds(1f);
            transform.localScale = Vector3.zero;
            textbox.text = "";
        }
    }
}
