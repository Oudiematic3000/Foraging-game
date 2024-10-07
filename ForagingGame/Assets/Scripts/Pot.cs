using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{
    public GameObject cookbookUI;
    public InventoryManager inventory;

    private void Start()
    {
        inventory = FindAnyObjectByType<InventoryManager>();
        cookbookUI = FindAnyObjectByType<CookBookManager>().gameObject;
    }
    public void ToggleCookbook()
    {
        if (cookbookUI.transform.localScale == Vector3.one)
        {
            cookbookUI.transform.localScale = Vector3.zero;
            inventory.isOpen = false;


        }
        else if (cookbookUI.transform.localScale == Vector3.zero)
        {
            cookbookUI.transform.localScale = Vector3.one;
            inventory.isOpen = true;


        }
    }
}
