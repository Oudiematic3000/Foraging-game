using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{
    public GameObject cookbookUI;
    public InventoryManager inventory;

    private void Start()
    {
       
        
    }
    public void ToggleCookbook()
    {
        inventory = FindAnyObjectByType<InventoryManager>();
        if (cookbookUI.transform.localScale == Vector3.one)
        {
            cookbookUI.transform.localScale = Vector3.zero;
            inventory.transform.localScale = Vector3.zero;
            inventory.isOpen = false;
            cookbookUI.GetComponent<CookBookManager>().isOpen = false;

        }
        else if (cookbookUI.transform.localScale == Vector3.zero)
        {
            inventory.transform.localScale = Vector3.one;
            cookbookUI.transform.localScale = Vector3.one;
            inventory.isOpen = true;
            cookbookUI.GetComponent <CookBookManager>().isOpen = true;

        }
    }
}
