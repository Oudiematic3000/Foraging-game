using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class IngredientSlot : MonoBehaviour
{
    public TextMeshProUGUI typeText;
    public Image ingredientImage;
    public Ingredient correctIngredient;
    public Ingredient guessIngredient;
    public State state=State.unvalidated;
    public CookBookManager cookBookManager;
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
    }

    private void OnMouseOver()
    {
        if(Input.GetMouseButtonUp(0))
        {
            setGuess(cookBookManager.dragIngredient);
        }
    }

    public void setGuess(Ingredient ingredientGuess)
    {
        guessIngredient = ingredientGuess;
        ingredientImage.sprite=ingredientGuess.sprite;
        ingredientImage.transform.localScale = Vector3.one;
        typeText.transform.localScale = Vector3.zero;
       
    }

    public void clearGuess()
    {
        guessIngredient = null;
        ingredientImage.sprite=null;
        ingredientImage.transform.localScale=Vector3.zero;
        typeText.transform.localScale=-Vector3.one;
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
