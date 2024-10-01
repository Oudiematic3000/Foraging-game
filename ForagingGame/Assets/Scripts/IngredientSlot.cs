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
    public TextMeshProUGUI typeText;
    public Ingredient correctIngredient;
    public Ingredient guessIngredient;
    public State state=State.unvalidated;
    public CookBookManager cookBookManager;
    public bool occupied=false;
    public static event Action childChanged;
    public static event Action<Ingredient> slotIngChanged;
    public enum State
    {
        correct,
        close,
        wrong,
        unvalidated
    }
    void Awake()
    {
        CookBookManager.startCook += checkGuess;
    }
    void Start()
    {
        
        cookBookManager = FindAnyObjectByType<CookBookManager>();

    }


    void Update()
    {

        
    }
    private void OnTransformChildrenChanged()
    {
        childChanged();
        if (transform.childCount == 1)
        {
            slotIngChanged(cookBookManager.itemHolder.GetComponentInChildren<InvItem>().ingredient);
            Destroy(gameObject);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!occupied)
        {
            if (cookBookManager.itemHolder.transform.GetChild(0))
            {
                occupied = true;
                cookBookManager.itemHolder.transform.GetChild(0).transform.SetParent(transform, false);
                guessIngredient = transform.GetChild(1).GetComponent<InvItem>().ingredient;
                typeText.transform.localScale = Vector3.zero;

            }
        }


    }



    public void clearGuess()
    {
        guessIngredient = null;
        typeText.transform.localScale=Vector3.one;
        StartCoroutine(delay());
        
    }

    public IEnumerator delay()
    {
        yield return new WaitForSeconds(0.1f);
        occupied = false;
    }
    public void checkGuess()
    {
        if(guessIngredient==correctIngredient)
        {
            state= State.correct;
        }else
        {
            if(correctIngredient.flavours.Length>guessIngredient.flavours.Length) {
                state = loopFlavoursLongCorrect();
            }
            else
            {
                state = loopFlavoursLongGuess();
            }
        }
    }

    public State loopFlavoursLongCorrect()
    {
        int wrongCount = 0;
        foreach (string correctFlavour in correctIngredient.flavours)
        {
            bool foundFlavour = false;
            bool searched = false;
            while (!foundFlavour && !searched)
            {
                foreach (string guessFlavour in guessIngredient.flavours)
                {
                    Debug.Log(guessFlavour + "guess vs correct " + correctFlavour);
                    if (guessFlavour == correctFlavour)
                    {
                        
                        foundFlavour = true;
                        break;
                    }

                }
                if (!foundFlavour) { wrongCount++; searched = true; } 
            }
            
        }
        if(wrongCount > 0 )
        {
            if (wrongCount == correctIngredient.flavours.Length)
            {
                Debug.Log(wrongCount);
                return State.wrong;
                
            }
            Debug.Log(wrongCount);
            return State.close;
        }
        else
        {
            Debug.Log(wrongCount);
            return State.correct;
        }
        
    }

    public State loopFlavoursLongGuess()
    {
        int wrongCount = 0;
        foreach (string guessFlavour in guessIngredient.flavours)
        {
            bool foundFlavour = false;
            bool searched = false;
            while (!foundFlavour && !searched)
            {
                foreach (string correctFlavour in correctIngredient.flavours)
                {
                    Debug.Log(guessFlavour + "guess vs correct " + correctFlavour);
                    if (guessFlavour == correctFlavour)
                    {

                        foundFlavour = true;
                        break;
                    }

                }
                if (!foundFlavour) { wrongCount++; searched = true; }
            }

        }
        if (wrongCount > 0)
        {
            if (wrongCount == guessIngredient.flavours.Length)
            {
                Debug.Log(wrongCount);
                return State.wrong;

            }
            Debug.Log(wrongCount);
            return State.close;
        }
        else
        {
            Debug.Log(wrongCount);
            return State.correct;
        }

    }
}
