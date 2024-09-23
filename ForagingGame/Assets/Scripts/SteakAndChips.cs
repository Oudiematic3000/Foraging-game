//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

//public class SteakAndChips
//{
//    public string recipeName;
//    public Element[] elements;
//    public int elemCount;


//    public SteakAndChips()
//    {
//        elements = new Element[3];
//        recipeName = "Steak and Chips";

//        Element steak = new Element("Steak", 3);
//        steak.addIngredient(Resources.Load<Ingredient>("Ingredients/MarbledBeef"));
//        steak.addIngredient(Resources.Load<Ingredient>("Ingredients/RockSalt"));
//        steak.addIngredient(Resources.Load<Ingredient>("Ingredients/VegetableOil"));
//        elements[0] = steak;

//        Element sauce = new Element("Sauce", 2);
//        sauce.addIngredient(Resources.Load<Ingredient>("Ingredients/Mushroom"));
//        sauce.addIngredient(Resources.Load<Ingredient>("Ingredients/BellPepper"));
//        elements[1] = sauce;

//        Element chips = new Element("Chips", 3);
//        chips.addIngredient(Resources.Load<Ingredient>("Ingredients/Potato"));
//        chips.addIngredient(Resources.Load<Ingredient>("Ingredients/RockSalt"));
//        chips.addIngredient(Resources.Load<Ingredient>("Ingredients/VegetableOil"));
//        elements[2] = chips;
//    }

//}



////public class Element
////{
    
////    public string name;
////    public Ingredient[] elemIngredients;
////    public int ingCount=0;
////    public Element(string n, int s)
////    {
////        name = n;
////        elemIngredients = new Ingredient[s];
////    }

////    public void addIngredient(Ingredient i)
////    {
////        elemIngredients[ingCount] = i;
////        ingCount++;
////    }

////    override public string ToString()
////    {
////        string output = "";
////        for(int i = 0; i < elemIngredients.Length; i++)
////        {
////            output += elemIngredients[i] + " ";
////        }
////        return output;
////    }
////}
