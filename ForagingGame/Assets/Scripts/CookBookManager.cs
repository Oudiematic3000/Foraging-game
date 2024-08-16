using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CookBookManager : MonoBehaviour
{
    public GameObject inventory;

    public TextMeshProUGUI recipeName;
    public TextMeshProUGUI Element1;
    public TextMeshProUGUI Element2;
    public TextMeshProUGUI Element3;


    public Button[] buttons1;
    public Button[] buttons2;
    public Button[] buttons3;

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
        transform.localScale = Vector3.zero;
        steakAndChips = new SteakAndChips();
        elem1Guesses = new Ingredient[steakAndChips.elements[0].ingCount];
        elem1Guesses = new Ingredient[steakAndChips.elements[1].ingCount];
        elem1Guesses = new Ingredient[steakAndChips.elements[2].ingCount];

        recipeName.text=steakAndChips.recipeName;
        Element1.text = steakAndChips.elements[0].name;
        Element2.text = steakAndChips.elements[1].name;
        Element3.text = steakAndChips.elements[2].name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void selectIngredient(int i)
    {
        inventory.transform.localScale = Vector3.one;
        switch (i)
        {
            case 1:
                target = 1;
                index=0; break;
            case 2:
                target = 1;
                index = 1; break;
            case 3:
                target = 1;
                index = 2; break;
            case 4:
                target = 2;
                index = 0; break;
            case 5:
                target = 2;
                index = 1; break;
            case 6:
                target = 3;
                index = 0; break;
            case 7:
                target = 3;
                index = 1; break;
            case 8:
                target = 3;
                index = 2; break;

        }
        if (waitForIngredient != null)
        {
            waitForIngredient(IngredientCallback);
            waiting=true;
        }
        
    }

    public void IngredientCallback(Ingredient ingredient)
    {
       
        Debug.Log(ingredient.ToString());
        SetGuesses(target, index, ingredient);
        waiting = false;
        inventory.transform.localScale = Vector3.zero;
    }
    public void SetGuesses(int i, int j, Ingredient ing)
    {
        switch(i)
        {
            case 1:
                if(ing.ingredientType== steakAndChips.elements[0].elemIngredients[j].ingredientType)
                {
                    
                    elem1Guesses[j] = ing;
                    buttons1[j].GetComponentInChildren<TextMeshProUGUI>().text = ing.ingredientName;
                }
                else
                {
                    StartCoroutine(BlinkRed(buttons1[j]));
                    Debug.Log("Wrong type (Needs: "+ steakAndChips.elements[0].elemIngredients[j].ingredientType);
                }
                
                break;
            case 2:
                if (ing.ingredientType == steakAndChips.elements[1].elemIngredients[j].ingredientType)
                {
                    elem2Guesses[j] = ing;
                    buttons2[j].GetComponentInChildren<TextMeshProUGUI>().text = ing.ingredientName;
                }
                else
                {
                    Debug.Log("Wrong type (Needs: " + steakAndChips.elements[1].elemIngredients[j].ingredientType);
                }
                break;
            case 3:
                if (ing.ingredientType == steakAndChips.elements[2].elemIngredients[j].ingredientType)
                {
                    elem3Guesses[j] = ing;
                    buttons3[j].GetComponentInChildren<TextMeshProUGUI>().text = ing.ingredientName;
                }
                else
                {
                    Debug.Log("Wrong type (Needs: " + steakAndChips.elements[2].elemIngredients[j].ingredientType);
                }
                break;
        }
    }

    public void checkIngredient(int k)
    {
        Ingredient[] targetGuessArray;
        Ingredient[] targetCheckArray;
        Button[] buttonArray;
        
        switch (k)
        {
            case 1:
                targetGuessArray = elem1Guesses;
                targetCheckArray = steakAndChips.elements[0].elemIngredients;
                buttonArray = buttons1;
                break;
            case 2:
                targetGuessArray = elem2Guesses;
                targetCheckArray = steakAndChips.elements[1].elemIngredients;
                buttonArray = buttons2;
                break;
            case 3:
                targetGuessArray = elem3Guesses;
                targetCheckArray = steakAndChips.elements[2].elemIngredients;
                buttonArray = buttons3;
                break;
            default:
                targetGuessArray = elem1Guesses;
                targetCheckArray = steakAndChips.elements[0].elemIngredients;
                buttonArray = buttons1;
                break;
        }

        for(int i = 0; i < targetGuessArray.Length; i++)
        {
            for(int j = 0; j < targetCheckArray.Length; j++)
            {
                if (targetGuessArray[i] == targetCheckArray[j])
                {
                    buttonArray[i].image.color = Color.green;
                    break;
                }
                
                {
                    int flavourMatch = 0;
                    if (targetGuessArray[i] != null) {
                        for (int m = 0; m < targetGuessArray[i].flavours.Length; m++)
                        {
                            
                            for (int n = 0; n < targetCheckArray[i].flavours.Length; n++)
                            {
                                Debug.Log(targetGuessArray[i].flavours[m] + " vs " + targetCheckArray[i].flavours[n]);
                                if (targetGuessArray[i].flavours[m] == targetCheckArray[i].flavours[n])
                                {
                                    flavourMatch++;
                                    buttonArray[i].image.color = Color.yellow;
                                    break;
                                }
                            }
                        }
                    }
                    
                    if(flavourMatch == 0)
                    {
                        buttonArray[i].image.color=Color.red;
                    }
                }
            }
        }
        
    }

    public IEnumerator BlinkRed(Button button)
    {
        button.image.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        button.image.color = Color.white;
        yield return new WaitForSeconds(0.2f);
        button.image.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        button.image.color = Color.white;


    }

}
