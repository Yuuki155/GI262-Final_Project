using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CookingRecipeSo", menuName = "Scriptable Objects/Cooking Recipe")]
public class CookingRecipeSo : ScriptableObject
{
    [SerializeField] SerializedDictionary<IngredientSo, int> Ingredient;
    [SerializeField] ItemSo Product;

    public IReadOnlyDictionary<IngredientSo, int> GetIngredients() => Ingredient;
    public ItemSo GetProduct() => Product;
    
}
