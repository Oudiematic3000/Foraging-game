using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvItem : MonoBehaviour
{
    public Ingredient ingredient;

    public int quantity;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUp(Ingredient item) 
    {
        this.ingredient = item;
        

    }
}
