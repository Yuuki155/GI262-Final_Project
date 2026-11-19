using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour, IInteractable
{
    [SerializeField] private bool isTalk = false;
    [SerializeField] private PatienceBar patienceBarUI;
    private Dictionary<FoodSo, int> OrderList = new Dictionary<FoodSo, int>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        patienceBarUI.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (patienceBarUI.isTimeUp == true)
        {
            RemoveCustomer();
        }
    }
    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player playerScript))
        {
            Hit(playerScript);
        }
    }*/
    public void Interact(Player player)
    {
        Hit(player);
    }
    public void Hit(Player player)
    {
        if (isTalk == false)
        {
            ShowOrder();
            patienceBarUI.gameObject.SetActive(true);
            isTalk = true;

            SpawnItem itemSpawner = FindFirstObjectByType<SpawnItem>();
            if (itemSpawner != null)
            {
                itemSpawner.SpawnOrderedItems(OrderList);
            }
            else
            {
                Debug.LogWarning("หาตัวจัดการ SpawnItem ในฉากไม่เจอ!");
            }
        }
        else
        {
            Check(player);
        }
    }
    private void ShowOrder()
    {
        foreach (var item in OrderList)
        {
            Debug.Log($"ลูกค้าสั่ง: {item.Key.GetItemName()} จำนวน {item.Value}");
        }
    }
    public Dictionary<FoodSo, int> GetOrderList()
    {
        return OrderList;
    }
    public void SetOrder(List<FoodSo> order, List<int> orderCount, int totalItems)
    {
        OrderList.Clear();
        for (int i = 0; i < totalItems; i++)
        {
            FoodSo currentOrder = order[i];
            int currentCount = orderCount[i];

            if (OrderList.ContainsKey(currentOrder))
            {
                OrderList[currentOrder] += currentCount;
            }
            else
            {
                OrderList.Add(currentOrder, currentCount);
            }
        }
    }
    public void RemoveCustomer()
    {
        Destroy(gameObject);
    }
    /*public void ResetOrder()
    {
        isTalk = false;
        OrderList.Clear();
        patienceBarUI.ResetcurrPatience();
        patienceBarUI.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }*/
    private void Check(Player player)
    {
        ItemSo heldItem = player.GetHeldItem();

        if (heldItem == null)
        {
            Debug.Log("ผู้เล่นไม่ได้ถืออาหารมา");
            return;
        }
        if (heldItem is FoodSo heldFood)
        {

            if (OrderList.ContainsKey(heldFood))
            {
                OrderList[heldFood]--;

                player.RemoveCurrentItem();

                Debug.Log($"เสิร์ฟ {heldFood} สำเร็จ!");
                if (OrderList[heldFood] == 0)
                {
                    OrderList.Remove(heldFood);
                }
                if (OrderList.Count == 0)
                {
                    Debug.Log("ลูกค้าได้รับอาหารครบแล้ว!");
                    RemoveCustomer();
                }
            }
            else
            {
                Debug.Log($"ลูกค้าไม่ได้สั่ง {heldFood}");
            }
        }
        else
        {
            Debug.Log($"สิ่งที่ถืออยู่ ({heldItem.GetItemName()}) ไม่ใช่อาหารที่เสิร์ฟได้");
        }

    }
}

