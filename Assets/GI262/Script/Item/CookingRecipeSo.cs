using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "CookingRecipeSo", menuName = "Scriptable Objects/Cooking Recipe")]
public class CookingRecipeSo : ScriptableObject
{
    [SerializeField] SerializedDictionary<IngredientSo, int> Ingredient;
    [SerializeField] ItemSo Product;
}
