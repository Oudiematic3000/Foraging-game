using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ElementSlot : MonoBehaviour, IPointerClickHandler
{
    public List<Ingredient> correctIngredients;
    public List<Ingredient> guessIngredients;
    public List<string> correctTypes;
    public List<string> guessTypes;
    [SerializeField]
    private List<string> wrongFlavs, missFlavs;


    public TextMeshProUGUI toAddList;

    public CookBookManager cookBookManager;
    public GameObject ingredientSlot, grid;

    void Awake()
    {
          IngredientSlot.childChanged += displayAdded;
        IngredientSlot.slotIngChanged += RemoveGuess;
    }
    void Start()
    {
        cookBookManager = GameObject.FindObjectOfType<CookBookManager>();
    }

    
    void Update()
    {
        
    }

    public void displayAdded()
    {
        string output = "";
        guessTypes.Clear();
        HashSet<Transform> matches = new HashSet<Transform>();

        foreach (string type in correctTypes)
        {
            
            bool found = false;
            foreach (Transform t in grid.transform)
            {
                if (t.childCount > 1)
                {


                    InvItem item = t.GetComponentInChildren<InvItem>();
                    if (matches.Contains(t)) continue;

                    if (item.ingredient.ingredientType.ToString() == type)
                    {
                        matches.Add(t);
                        found = true;
                        break;
                    }
                }
            }
            if (!found) output += "1x " + type + "\n";
        }
        toAddList.text = output;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
            if (cookBookManager.itemHolder.transform.childCount>0)
            {
            AddIngredient(cookBookManager.itemHolder.transform.GetChild(0).GetComponent<InvItem>().ingredient);
            var newSlot =Instantiate(ingredientSlot, grid.transform);
            cookBookManager.itemHolder.transform.GetChild(0).transform.SetParent(newSlot.transform, false);
            }
       
    }

    public void Setup(Ingredient ing)
    {
        correctIngredients.Add(ing);
        correctTypes.Add(ing.ingredientType.ToString());
    }

    public void AddIngredient(Ingredient ing)
    {
        Debug.Log(ing.ingredientName +"added");
        guessIngredients.Add(ing);
        guessTypes.Add(ing.ingredientType.ToString());
        displayAdded();
        CheckGuess();
    }

    public void RemoveGuess(Ingredient ing)
    {
        guessIngredients.Remove(ing);
        displayAdded(); CheckGuess();
    }

    public void CheckGuess()
    {
        wrongFlavs.Clear();
        missFlavs.Clear();
        List<string> cFlavours = new List<string>();
        List<string> gFlavours = new List<string>();
        foreach(Ingredient ing in correctIngredients)
        {
            foreach(string flav in ing.flavours)
            {
                cFlavours.Add(flav);
            }
        }
        foreach (Ingredient ing in guessIngredients)
        {
            foreach (string flav in ing.flavours)
            {
                gFlavours.Add(flav);
            }
        }
        compareFlavours(cFlavours, gFlavours);

    }

    public void compareFlavours(List<string> correct, List<string> guess)
    {
            foreach (string sFlav in guess)
            {
                if (!correct.Contains(sFlav))
                {
                    wrongFlavs.Add(sFlav);
                }
            }
        
            foreach (string lFlav in correct)
            {
                if (!guess.Contains(lFlav))
                {
                    missFlavs.Add(lFlav);
                }
            }
        

    }

   

  



}
