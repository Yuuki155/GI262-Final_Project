using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class WaveData
{
    public string waveName = "Wave 1";
    public int customerCount = 3;
    public float timeBetweenSpawns = 5f;
    public List<FoodSo> foodList = new List<FoodSo>();
    public int maxMenuTypes = 1;
    public int maxQuantityPerItem = 1;
}
public class WaveManager : MonoBehaviour
{


    [Header("Set Customer")]
    [SerializeField] private Customer customerPrefab;
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();

    [Header("Set all waves")]
    [SerializeField] private List<WaveData> waves = new List<WaveData>();
    [SerializeField] private float timeBetweenWaves = 10f;

    private int currentWaveIndex = 0;
    private bool isSpawningWave = false;
    private List<Transform> availableSpawnPoints = new List<Transform>();
    void Start()
    {
        StartNextWave();
    }
    public void StartNextWave()
    {
        if (currentWaveIndex < waves.Count && !isSpawningWave)
        {
            StartCoroutine(SpawnWaveRoutine(waves[currentWaveIndex]));
        }
        else if (currentWaveIndex >= waves.Count)
        {
            Debug.Log("All Waves Cleared");
        }
    }
    private IEnumerator SpawnWaveRoutine(WaveData wave)
    {
        isSpawningWave = true;
        Debug.Log($"Start: {wave.waveName}");

        for (int i = 0; i < wave.customerCount; i++)
        {
            SpawnCustomerWithOrder(wave);
            yield return new WaitForSeconds(wave.timeBetweenSpawns);
        }

        isSpawningWave = false;
        currentWaveIndex++;

        if (currentWaveIndex < waves.Count)
        {
            Debug.Log($"End Wave! พัก {timeBetweenWaves} วินาที...");
            yield return new WaitForSeconds(timeBetweenWaves);
            StartNextWave();
        }
        else
        {
            Debug.Log("ผ่านครบทุก Wave แล้ว!");
        }
    }
    private void SpawnCustomerWithOrder(WaveData wave)
    {
        if (wave.foodList == null || wave.foodList.Count == 0)
        {
            Debug.LogWarning("ยังไม่ได้ใส่เมนูใน Allowed Menus ของ Wave นี้!");
            return;
        }
        if (availableSpawnPoints.Count == 0)
        {
            availableSpawnPoints.AddRange(spawnPoints);
        }
        int randomIndex = Random.Range(0, availableSpawnPoints.Count);
        Transform chosenSpawnPoint = availableSpawnPoints[randomIndex];
        availableSpawnPoints.RemoveAt(randomIndex);
        Customer newCustomer = Instantiate(customerPrefab, chosenSpawnPoint.position, chosenSpawnPoint.rotation);

        int totalItems = Random.Range(1, wave.maxMenuTypes + 1);
        List<FoodSo> selectedOrders = new List<FoodSo>();
        List<int> selectedCounts = new List<int>();

        for (int i = 0; i < totalItems; i++)
        {
            FoodSo randomFood = wave.foodList[Random.Range(0, wave.foodList.Count)];
            int randomCount = Random.Range(1, wave.maxQuantityPerItem + 1);

            selectedOrders.Add(randomFood);
            selectedCounts.Add(randomCount);
        }

        newCustomer.SetOrder(selectedOrders, selectedCounts, totalItems);
    }
}
