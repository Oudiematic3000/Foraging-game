using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public enum State
    {
        correct,
        close,
        wrong,
        unvalidated
    }
    void Start()
    {
        checkGuess();
        cookBookManager = FindAnyObjectByType<CookBookManager>();
    }

    void Update()
    {

        
    }
    private void OnTransformChildrenChanged()
    {
        clearGuess();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!occupied)
        {
            if (cookBookManager.itemHolder.transform.GetChild(0))
            {
                cookBookManager.itemHolder.transform.GetChild(0).transform.SetParent(transform, false);
                guessIngredient = transform.GetChild(1).GetComponent<InvItem>().ingredient;
                typeText.transform.localScale = Vector3.zero;
                occupied = true;
            }
        }
       
       
    }
  
    

    public void clearGuess()
    {
        guessIngredient = null;
        typeText.transform.localScale=Vector3.one;
        
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
