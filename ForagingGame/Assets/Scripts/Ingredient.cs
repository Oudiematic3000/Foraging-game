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
    public List<string> unrevealedFlavours, revealedFlavours;

    private void OnEnable()
    {
        unrevealedFlavours.Clear();
        revealedFlavours.Clear();
        foreach(string flavour in flavours) unrevealedFlavours.Add(flavour);
    }
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

    public void tasteIngredient()
    {
        int rn = Random.Range(0, unrevealedFlavours.Count);
        revealedFlavours.Add(unrevealedFlavours[rn]);
        unrevealedFlavours.RemoveAt(rn);
    }

    public string toString()
    {
        string output = ingredientName + " - " + ingredientType + "\t<b>";
        foreach(string flav in revealedFlavours)
        {
            output += "\n" + flav + " flavour...";
        }
        output+="</b>\n" + ingredientDescription;
        return output;
    }
}
