using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CookBookManager : MonoBehaviour
{
    public GameObject inventory;

    public TextMeshProUGUI recipeName;
   
    public TMP_Dropdown dropdown;
    public GameObject elementDisplay, ingredientButton, itemHolder, target;
    public Ingredient dragIngredient;
    GraphicRaycaster gr;

    public bool holding;


    private void Start()
    {
        AddRecipe("Steak and Chips");
        dropdown.captionText.text = "Steak and Chips";
        displayRecipe();
        gr = FindAnyObjectByType<Canvas>().GetComponent<GraphicRaycaster>();
    }

    public void Update()
    {
        itemHolder.transform.position=Input.mousePosition+new Vector3(75,-75,0);
        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData ptr = new PointerEventData(EventSystem.current);
            ptr.position =Input.mousePosition;
            
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(ptr, results);
            
            foreach(RaycastResult result in results )
            {
                Debug.Log(result.gameObject.name);
                if(result.gameObject.GetComponent<InvItem>()) target=result.gameObject;
            }
            if (target)
            {
                
              GameObject heldItem=  Instantiate(target, itemHolder.transform);
                target.GetComponent<InvItem>().RemoveItem();
                target.GetComponent<InvItem>().RefreshCount();
                heldItem.GetComponent<InvItem>().itemCount = 1;
                heldItem.GetComponent<InvItem>().RefreshCount();
            }
            target = null;
        }
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
   
