using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InvItem : MonoBehaviour, IPointerEnterHandler
{
    public Ingredient ingredient;
    public int quantity;
    public TextMeshProUGUI itemText;
    public int itemCount = 1;
    public bool selected = false;

    void Start()
    {
        RefreshCount();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUp(Ingredient item) 
    {
        this.ingredient = item;
        

    }
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        displayInfo();
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
