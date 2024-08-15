using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SteakAndChips
{
    public string recipeName;
    public Element[] elements = new Element[3];
    public int elemCount;


    public SteakAndChips()
    {
        Element steak = new Element("Steak", 3);
        steak.addIngredient(AssetDatabase.LoadAssetAtPath<Ingredient>("Assets/Ingredients/MarbledBeef.asset"));
        steak.addIngredient(AssetDatabase.LoadAssetAtPath<Ingredient>("Assets/Ingredients/RockSalt.asset"));
        steak.addIngredient(AssetDatabase.LoadAssetAtPath<Ingredient>("Assets/Ingredients/VegetableOil.asset"));
        elements[0] = steak;

        Element sauce = new Element("Sauce", 2);
        sauce.addIngredient(AssetDatabase.LoadAssetAtPath<Ingredient>("Assets/Ingredients/Mushroom.asset"));
        sauce.addIngredient(AssetDatabase.LoadAssetAtPath<Ingredient>("Assets/Ingredients/BellPepper.asset"));
        elements[1] = sauce;

        Element chips = new Element("Chips", 2);
        chips.addIngredient(AssetDatabase.LoadAssetAtPath<Ingredient>("Assets/Ingredients/Potato.asset"));
        chips.addIngredient(AssetDatabase.LoadAssetAtPath<Ingredient>("Assets/Ingredients/VegetableOil.asset"));
        elements[2] = chips;
    }

}



public class Element
{
    
    public string name;
    public Ingredient[] elemIngredients;
    public int ingCount=0;
    public Element(string n, int s)
    {
        name = n;
        elemIngredients = new Ingredient[s];
    }

    public void addIngredient(Ingredient i)
    {
        elemIngredients[ingCount] = i;
    }
}
