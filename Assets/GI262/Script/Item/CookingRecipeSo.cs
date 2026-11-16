using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CookingRecipeSo", menuName = "Scriptable Objects/Cooking Recipe")]
public class CookingRecipeSo : ScriptableObject
{
    [SerializeField] List<IngredientSo> Ingredient;
    [SerializeField] ItemSo Product;
}
