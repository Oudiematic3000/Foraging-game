using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InvItem : MonoBehaviour
{
    public Ingredient ingredient;
    public int quantity;
    public TextMeshProUGUI itemText;
    public int itemCount = 1;


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

    public void RefreshCount()
    {
        itemText.text = itemCount.ToString();
        bool textActive = itemCount > 1;
        itemText.gameObject.SetActive(textActive);
    }
}
