using System.Collections;
using System.Collections.Generic;
using System;
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
    public GameObject elementDisplay,  itemHolder, target;
    public Ingredient dragIngredient;
    GraphicRaycaster gr;
    public static event Action startCook;
    public static event Action SendFeedback;
    public static event Action ConsumeIngredients;
    public bool isOpen=false;


    private void Start()
    {
        AddRecipe("Steak and Chips");
        dropdown.captionText.text = "Steak and Chips";
        displayRecipe();
        gr = FindAnyObjectByType<Canvas>().GetComponent<GraphicRaycaster>();
        transform.localScale= Vector3.zero;
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
                if(result.gameObject.GetComponent<InvItem>()) target=result.gameObject;
            }
            if (target && itemHolder.transform.childCount==0)
            {
                
              GameObject heldItem=  Instantiate(target, itemHolder.transform);
                target.GetComponent<InvItem>().RemoveItem();
                target.GetComponent<InvItem>().RefreshCount();
                heldItem.GetComponent<InvItem>().itemCount = 1;
                heldItem.GetComponent<InvItem>().RefreshCount();
            }else if (target)
            {
                Ingredient held = itemHolder.transform.GetChild(0).GetComponent<InvItem>().ingredient;
                inventory.GetComponent<InventoryManager>().AddInventory(held);
                Destroy(itemHolder.transform.GetChild(0).gameObject);

                GameObject heldItem = Instantiate(target, itemHolder.transform);
                target.GetComponent<InvItem>().RemoveItem();
                target.GetComponent<InvItem>().RefreshCount();
                heldItem.GetComponent<InvItem>().itemCount = 1;
                heldItem.GetComponent<InvItem>().RefreshCount();
            }
            target = null;
        }
        if(Input.GetMouseButtonDown(1) && itemHolder.transform.childCount>0) {
        
            Ingredient held =itemHolder.transform.GetChild(0).GetComponent<InvItem>().ingredient;
            inventory.GetComponent<InventoryManager>().AddInventory(held);
            Destroy(itemHolder.transform.GetChild(0).gameObject);
        }
    }

    public void startCookFunc()
    {
        startCook();
        SendFeedback();
        ConsumeIngredients();
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
            ElementSlot newElemScript = newElem.GetComponent<ElementSlot>();
            newElemScript.ElementTitle.text=e.elementName;
            foreach (Ingredient ing in e.elementIngredients)
            {
                newElemScript.Setup(ing);
            }
            newElemScript.displayAdded();
            i++;
        }
    }

    

   
}
   
