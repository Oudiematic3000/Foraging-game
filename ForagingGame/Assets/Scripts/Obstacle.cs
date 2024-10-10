using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    public Ingredient.Tool toolneeded;
    public Animator animator;
    public float fadeDuration = 1f;
    private Material[] materials;

    void Start()
    {
        
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        materials = new Material[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            materials[i] = renderers[i].material;
        }
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        yield return new WaitForSeconds(1f);
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);

            foreach (Material mat in materials)
            {
                Debug.Log(mat);
                Color color = mat.color;
                Debug.Log(mat.color.a);
                color.a = alpha;
                mat.color = color;
            }

            yield return null;
        }
        Destroy(gameObject);
    }
    public void PlayDeath()
    {
        GetComponent<BoxCollider>().enabled = false;
        StartCoroutine(FadeOut());
        animator.Play("Scene");
        
    }
}
