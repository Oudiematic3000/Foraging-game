using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
 
    public void loadingScene(List<AsyncOperation> loadings)
    {
        gameObject.SetActive(true);
        animator.Play("FadeIn");
        foreach (AsyncOperation loading in loadings)
        {
            while (!loading.isDone)
            {
               continue;
            }
        }
        animator.Play("FadeOut");
        
    }
}
