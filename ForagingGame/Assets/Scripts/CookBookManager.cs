using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CookBookManager : MonoBehaviour
{
    public TextMeshProUGUI recipeName;
    public TextMeshProUGUI Element1;
    public TextMeshProUGUI Element2;
    public TextMeshProUGUI Element3;
    public SteakAndChips steakAndChips; //This will eventually be a declared recipe class which various recipes derive from but that's not necessary for prototype.
    public delegate void clickDelegate(System.Action<Ingredient> callback);
    public static event clickDelegate waitForIngredient;
    public bool waiting;

    int target;
    int index;
    public Ingredient[] elem1Guesses;
    public Ingredient[] elem2Guesses;
    public Ingredient[] elem3Guesses;




    void Start()
    {
        steakAndChips = new SteakAndChips();
        elem1Guesses = new Ingredient[steakAndChips.elements[0].ingCount];
        elem1Guesses = new Ingredient[steakAndChips.elements[1].ingCount];
        elem1Guesses = new Ingredient[steakAndChips.elements[2].ingCount];


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void selectIngredient(int i, int j)
    {
        target = i;
        index = j;
        if (waitForIngredient != null)
        {
            waitForIngredient(IngredientCallback);
            waiting=true;
        }
        
    }

    public void IngredientCallback(Ingredient ingredient)
    {
        waiting = false;
        SetGuesses(target, index, ingredient);
    }
    public void SetGuesses(int i, int j, Ingredient ing)
    {
        switch(i)
        {
            case 1:
                if(ing.ingredientType== steakAndChips.elements[0].elemIngredients[j].ingredientType)
                {
                    elem1Guesses[j] = ing;
                }
                else
                {
                    Debug.Log("Wrong type");
                }
                
                break;
            case 2:
                if (ing.ingredientType == steakAndChips.elements[1].elemIngredients[j].ingredientType)
                {
                    elem2Guesses[j] = ing;
                }
                else
                {
                    Debug.Log("Wrong type");
                }
                break;
            case 3:
                if (ing.ingredientType == steakAndChips.elements[2].elemIngredients[j].ingredientType)
                {
                    elem3Guesses[j] = ing;
                }
                else
                {
                    Debug.Log("Wrong type");
                }
                break;
        }
    }

    public void checkIngredient(int i, Ingredient ing)
    {
        
    }
    
}
