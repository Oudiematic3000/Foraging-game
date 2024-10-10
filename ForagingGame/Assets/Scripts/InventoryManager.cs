using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public Sprite openSprite;
    public GameObject cookSprite;
    public static InventoryManager instance;
    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

        void Start()
    {
        transform.localScale = Vector3.zero;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOpen)
        {
            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true;
        }

        if (FindAnyObjectByType<CookBookManager>().isOpen)
        {
            cookSprite.SetActive(true);
            GameObject.Find("FeedbackButton").transform.localScale = Vector3.zero;
            GetComponent<Image>().enabled=false;
            GameObject.Find("Description").GetComponent<TextMeshProUGUI>().color = Color.white;
        }
        else
        {
            GetComponent<Image>().enabled=true;
            GameObject.Find("FeedbackButton").transform.localScale = Vector3.one;
            cookSprite.SetActive(false);
            GameObject.Find("Description").GetComponent<TextMeshProUGUI>().color = Color.black;
        }

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
            if (transform.localScale != Vector3.zero) { Instantiate(invItem, FindSlot().transform.position, Quaternion.identity, FindSlot().transform); }
            else
            {
                this.transform.localScale = Vector3.one;
                Instantiate(invItem, FindSlot().transform.position, Quaternion.identity, FindSlot().transform);
                this.transform.localScale = Vector3.zero;
            }
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
