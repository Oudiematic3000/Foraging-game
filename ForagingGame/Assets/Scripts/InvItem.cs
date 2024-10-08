using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Unity.VisualScripting;

public class InvItem : MonoBehaviour, IPointerEnterHandler
{
    public Ingredient ingredient;
    public TextMeshProUGUI itemText;
    public int itemCount = 1;
    public Image itemSprite;
    void Start()
    {
        RefreshCount();
    }


    void Update()
    {
        if (FindAnyObjectByType<CookBookManager>().isOpen)
        {
            itemText.color = Color.white;
        }
        else
        {
            itemText.color = Color.black;
        }
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

   


    public void RefreshCount()
    {
        itemText.text = itemCount.ToString();
        bool textActive = itemCount > 1;
        itemText.gameObject.SetActive(textActive);
        if (itemCount < 1) Destroy(gameObject);
    }
    public void RemoveItem()
    {
        itemCount--;
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
