using NUnit.Framework.Internal.Execution;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private Item itemPrefab;
    [SerializeField] private Transform spawnArea;
    [SerializeField] private float spawnRadius = 4.5f;
    private Customer customer;

    void Awake()
    {
        customer = GetComponent<Customer>();
    }

    /*void Update()
    {
        if (customer == null || !customer.spawnItem) return;
        SpawnOrderedItems();
        customer.spawnItem = false;
    }*/

    public void SpawnOrderedItems(Dictionary<FoodSo, int> orders)
    {
        if (itemPrefab == null)
        {
            Debug.LogError("Error: ยังไม่ได้ใส่ Item Prefab ในสคริปต์ SpawnItem!");
            return;
        }
        //Dictionary<FoodSo, int> orders = customer.GetOrderList();
        foreach (var order in orders)
        {
            FoodSo foodData = order.Key;
            int quantity = order.Value;
            for (int i = 0; i < quantity; i++)
            {
                Vector3 basePosition = spawnArea != null ? spawnArea.position : transform.position;
                Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
                Vector3 spawnPos = basePosition + new Vector3(randomOffset.x, randomOffset.y, 0f);
                Item spawnedItem = Instantiate(itemPrefab, spawnPos, Quaternion.identity);
                spawnedItem.InitializeItem(foodData);
                Debug.Log($"เสก {foodData.GetItemName()} สำเร็จที่ตำแหน่ง {spawnPos}");
            }
        }
    }
}
