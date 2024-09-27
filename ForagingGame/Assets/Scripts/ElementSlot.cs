using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementSlot : MonoBehaviour
{
    public List<Ingredient> correctIngredients;
    public List<Ingredient> guessIngredients;
    public List<string> correctTypes;
    public List<string> guessTypes;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void addIngredient(Ingredient ing)
    {
        guessIngredients.Add(ing);
        guessTypes.Add(ing.ingredientType.ToString());
    }
}
