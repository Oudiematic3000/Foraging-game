using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ingredient", menuName ="ingredient")]
public class Ingredient : ScriptableObject
{
    public string ingredientName;
    public string[] flavours;
    public string ingredientType;
    public string ingredientDescription;

}
