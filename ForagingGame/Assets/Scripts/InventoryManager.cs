using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject[] slots;
     public InvItem invItem;
    public bool isOpen=false;

    void Start()
    {
        transform.localScale = Vector3.zero;
        
    }

    // Update is called once per frame
    void Update()
    {
        

    }
    public void AddInventory(Ingredient item)
    {
        bool isDuplicate = false;
        for (int i =0; i<slots.Length; i++)
        {
        
            InvItem slotItem = slots[i].GetComponentInChildren<InvItem>();
            if(slotItem != null && slotItem.ingredient == item)
            {
                slotItem.itemCount++;
                slotItem.RefreshCount();   
                isDuplicate = true;
            }
        }
        if (isDuplicate == false)
        {
            invItem.SetUp(item);
            this.transform.localScale = Vector3.one;
            Instantiate(invItem, FindSlot().transform.position, Quaternion.identity, FindSlot().transform);
            this.transform.localScale = Vector3.zero;
        }
    }
    private GameObject FindSlot()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].transform.childCount == 0)
            {
                return slots[i];
            }
           
        }
        
        return slots[0];
        
    }

    
}
