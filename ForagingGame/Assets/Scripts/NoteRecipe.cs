using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NoteRecipe : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public TextMeshProUGUI noteText;
    // Start is called before the first frame update
    void Start()
    {
        dropdown.captionText.text = "Steak and Chips";
        DisplayRecipe();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (FindAnyObjectByType<CookBookManager>().isOpen)
        {
            transform.localScale = Vector3.zero;
        }
        else if(GameObject.FindAnyObjectByType<Feedback>().transform.localScale == Vector3.zero)
        {
            transform .localScale = Vector3.one;
        }
    }

    public void Toggle()
    {
        if (transform.localScale == Vector3.zero)
        {
            transform.localScale = Vector3.one;
        }
        else
        {
            transform.localScale = Vector3.zero;
        }
    }

    public void DisplayRecipe()
    {
        
        string output = "";
        Recipe displayedRecipe = Resources.Load<Recipe>("Recipes/" + dropdown.captionText.text);
        foreach(Element e in displayedRecipe.elements)
        {
            output += "<size=150%>" + e.elementName + "<size=100%>\n";
            foreach(Ingredient ing in e.elementIngredients)
            {
                output += "-" + ing.ingredientType.ToString() + "\n";
            }
        
        }
        noteText.text = output;
    }
}
