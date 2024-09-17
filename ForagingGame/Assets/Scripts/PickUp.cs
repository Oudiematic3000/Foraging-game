using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp: MonoBehaviour
{
    public Ingredient ingredient;
  
    void Start()
    {
        this.name=ingredient.ingredientName;
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
