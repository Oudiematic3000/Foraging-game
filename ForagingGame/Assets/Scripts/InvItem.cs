using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Unity.VisualScripting;

public class InvItem : MonoBehaviour, IPointerEnterHandler, IBeginDragHandler, IEndDragHandler
{
    public Ingredient ingredient;
    public int quantity;
    public TextMeshProUGUI itemText;
    public int itemCount = 1;
    public bool selected = false;
    public Image itemSprite;
    public bool waitingSelection=false;
    public event Action<GameObject> itemClicked;
    void Start()
    {
        RefreshCount();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnEnable()
    {
        
    }
    

    public void SetUp(Ingredient item) 
    {
        this.ingredient = item;
        itemSprite.sprite=ingredient.sprite;
    }


    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        displayInfo();
    }

    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        CookBookManager cookbook = GameObject.Find("Cookbook").GetComponent<CookBookManager>();
        cookbook.dragIngredient=ingredient;
        Debug.Log(cookbook.dragIngredient);
    }
    
    public void OnEndDrag(PointerEventData pointerEventData)
    {
        CookBookManager cookbook = GameObject.Find("Cookbook").GetComponent<CookBookManager>();
        Ray ray=new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            if (!hit.collider.GetComponent<IngredientSlot>())
            {
                InventoryManager inventory= GameObject.FindAnyObjectByType<InventoryManager>();
                inventory.AddInventory(cookbook.dragIngredient);
            }
        }
        else
        {
            InventoryManager inventory = GameObject.FindAnyObjectByType<InventoryManager>();
            inventory.AddInventory(cookbook.dragIngredient);
        }
    }


    public void RefreshCount()
    {
        itemText.text = itemCount.ToString();
        bool textActive = itemCount > 1;
        itemText.gameObject.SetActive(textActive);
    }

    public void displayInfo()
    {
        TextMeshProUGUI descriptionPane = GameObject.Find("Description").GetComponent<TextMeshProUGUI>();
        descriptionPane.text=ingredient.toString();
    }

    private void OnMouseEnter()
    {
        displayInfo();
    }
}
