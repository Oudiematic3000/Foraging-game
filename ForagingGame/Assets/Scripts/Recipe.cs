using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Recipe", menuName ="Recipe")]
public class Recipe : ScriptableObject
{
    public Element[] elements;
}

[System.Serializable]
public class Element
{
    public string name;
    public Ingredient[] elementIngredients;
    
}
