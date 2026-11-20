using AYellowpaper.SerializedCollections;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private Item itemPrefab;
    [SerializeField] private Transform spawnArea;
    [SerializeField] private float spawnRadius = 4.5f;
    
    [Header("Recipe Database")]
    [SerializeField] private List<CookingRecipeSo> allRecipes = new List<CookingRecipeSo>();
    
    private FieldInfo ingredientField;
    private FieldInfo productField;

    
    void Awake()
    {
        ingredientField = typeof(CookingRecipeSo).GetField("Ingredient", BindingFlags.NonPublic | BindingFlags.Instance);
        productField = typeof(CookingRecipeSo).GetField("Product", BindingFlags.NonPublic | BindingFlags.Instance);
    }
    public void SpawnOrderedItems(Dictionary<FoodSo, int> orders)
    {
        if (itemPrefab == null)
        {
            Debug.LogError("Error: ยังไม่ได้ใส่ Item Prefab ในสคริปต์ SpawnItem!");
            return;
        }

        // วนลูปอาหารแต่ละเมนูที่ลูกค้าสั่ง
        foreach (var order in orders)
        {
            FoodSo orderedFood = order.Key;
            int foodQuantity = order.Value;

            if (FindRecipeForFood(orderedFood) == null)
            {
                Debug.LogWarning($"ไม่พบสูตรอาหารสำหรับเมนู: {orderedFood.GetItemName()} ในช่อง All Recipes!");
                continue;
            }
            ResolveAndSpawn(orderedFood, foodQuantity);
        }
    }

    private void ResolveAndSpawn(ItemSo targetItem, int multiplierQuantity)
    {
        CookingRecipeSo recipe = FindRecipeForFood(targetItem);

        // แตกสูตรย่อยลงไปอีกชั้น
        if (recipe != null)
        {
            var ingredients = ingredientField.GetValue(recipe) as SerializedDictionary<IngredientSo, int>;

            if (ingredients != null)
            {
                foreach (var ing in ingredients)
                {
                    IngredientSo subIngredient = ing.Key;
                    int requiredAmount = ing.Value * multiplierQuantity;

                    // เรียกตัวเองซ้ำเพื่อเช็คว่าต้องแตกสูตรต่ออีกไหม
                    ResolveAndSpawn(subIngredient, requiredAmount);
                }
            }
        }
        // ไอเทมนี้ไม่มีสูตรผสมแล้ว
        else
        {
            for (int i = 0; i < multiplierQuantity; i++)
            {
                SpawnSingleItem(targetItem);
            }
        }
    }

    private CookingRecipeSo FindRecipeForFood(ItemSo targetFood)
    {
        foreach (CookingRecipeSo recipe in allRecipes)
        {
            if (recipe == null) continue;

            // เทียบว่าตรงกับอาหารที่ลูกค้าสั่งหรือไม่
            ItemSo product = productField.GetValue(recipe) as ItemSo;
            if (product == targetFood)
            {
                return recipe;
            }
        }
        return null;
    }

    private void SpawnSingleItem(ItemSo itemData)
    {
        Vector3 basePosition = spawnArea != null ? spawnArea.position : transform.position;
        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = basePosition + new Vector3(randomOffset.x, randomOffset.y, 0f);

        Item spawnedItem = Instantiate(itemPrefab, spawnPos, Quaternion.identity);
        spawnedItem.InitializeItem(itemData);
    }
}
