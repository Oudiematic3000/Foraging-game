using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp: MonoBehaviour
{
    public Ingredient ingredient;
  
    void Start()
    {
        this.name=ingredient.ingredientName;
        this.GetComponent<Renderer>().material=ingredient.material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
