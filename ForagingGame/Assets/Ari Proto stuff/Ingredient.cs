using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ingredient", menuName ="ingredient")]
public class Ingredient : ScriptableObject
{

    public string ingredientName;
    public string[] flavours;
    public Type ingredientType;
    public string ingredientDescription;
    public Tool toolNeeded;
    public Mesh mesh;

    public enum Tool
    {
        None,
        Drill
    }

    public enum Type
    {
        Protein,
        Seasoning,
        Produce,
        Starch,
        Fat
    }
}
