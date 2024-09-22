using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="ingredient", menuName ="ingredient")]
public class Ingredient : ScriptableObject
{

    public string ingredientName;
    public string[] flavours;
    public Type ingredientType;
    public string ingredientDescription;
    public Tool toolNeeded;
    public Sprite sprite;
    

    public enum Tool
    {
        None,
        Drill,
        Scraper
    }

    public enum Harvest
    {
        Take,
        Drill,
        Scrape
    }

    public enum Type
    {
        Protein,
        Seasoning,
        Produce,
        Starch,
        Fat
    }

    public string toString()
    {
        return ingredientName + "\n\n" +ingredientType+"\n\n"+ ingredientDescription;
    }
}
