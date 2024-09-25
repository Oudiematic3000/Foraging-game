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
   
    public TMP_Dropdown dropdown;
    public GameObject elementDisplay, ingredientButton;
    public Ingredient dragIngredient;

   
    public bool waiting;


    private void Start()
    {
        AddRecipe("Steak and Chips");
        dropdown.captionText.text = "Steak and Chips";
       displayRecipe();
    }

    public void AddRecipe(string recipeName)
    {
       
        dropdown.options.Add(new TMP_Dropdown.OptionData() { text=recipeName});
    }

    public void displayRecipe()
    {
        Debug.Log(dropdown.captionText.text);
        Recipe displayedRecipe = Resources.Load<Recipe>("Recipes/" + dropdown.captionText.text);
        Debug.Log(displayedRecipe);
        int i = 1;
        
        foreach(Element e in displayedRecipe.elements)
        {
          var newElem= Instantiate(elementDisplay, GameObject.Find("Content").transform.position + new Vector3(0, 159-(i*331),0),Quaternion.identity,GameObject.Find("Content").transform);
            int j = 0;
            foreach (Ingredient ing in e.elementIngredients)
            {
                Instantiate(ingredientButton, newElem.transform.position+new Vector3(-450+(j*100), 0, 0), Quaternion.identity, newElem.transform);
                j++;
            }
            i++;
        }
    }

    

   
}
   
