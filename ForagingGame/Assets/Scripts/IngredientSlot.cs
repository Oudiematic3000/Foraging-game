using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IngredientSlot : MonoBehaviour, IPointerClickHandler
{
    public CookBookManager cookBookManager;
    public bool occupied=false;
    public static event Action childChanged;
    public static event Action<Ingredient> slotIngChanged;
    
    
    void Start()
    {
        cookBookManager = FindAnyObjectByType<CookBookManager>();
    }


    
    private void OnTransformChildrenChanged()
    {
        
        if (transform.childCount == 1)
        {
           if(cookBookManager.itemHolder.transform.childCount>0) slotIngChanged(cookBookManager.itemHolder.GetComponentInChildren<InvItem>().ingredient);
           Destroy(gameObject);
        }
        childChanged();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!occupied)
        {
            if (cookBookManager.itemHolder.transform.GetChild(0))
            {
                occupied = true;
                
            }
        }


    }



    public void clearGuess()
    {
        StartCoroutine(delay());
    }

    public IEnumerator delay()
    {
        yield return new WaitForSeconds(0.1f);
        occupied = false;
    }
    
}
