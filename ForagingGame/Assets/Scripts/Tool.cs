using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    public Ingredient.Tool tool;
    // Start is called before the first frame update
    void Start()
    {
        if (FindAnyObjectByType<FirstPersonControls>().ownedTools.Contains(tool))
        {
            Destroy(gameObject);
        }
    }

}
