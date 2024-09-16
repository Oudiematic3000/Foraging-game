using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvItem : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public Ingredient ingredient;
    public int quantity;
    public TextMeshProUGUI itemText;
    public int itemCount = 1;
    public bool selected = false;
    public Image itemSprite;
    public bool waitingSelection=false;
    public System.Action<Ingredient> callback;
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
        CookBookManager.waitForIngredient += waitForIngredient;
    }
    public void waitForIngredient(System.Action<Ingredient> callback)
    {
        this.callback=callback;
    }

    public void SetUp(Ingredient item) 
    {
        this.ingredient = item;
        
    }


    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        displayInfo();
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        CookBookManager cookbook = GameObject.Find("Cookbook").GetComponent<CookBookManager>();
        if(callback!=null && cookbook.waiting)
        {
            callback.Invoke(ingredient);
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
