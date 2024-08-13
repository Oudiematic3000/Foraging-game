using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class InventoryManager : MonoBehaviour
{
    public GameObject[] slots;
     public InvItem invItem;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddInventory(Ingredient item)
    {
        invItem.SetUp(item);
        Instantiate(invItem, FindSlot().transform.position,Quaternion.identity, FindSlot().transform);
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
