using UnityEngine;
using System.Collections.Generic;

public class SpawnerManager : MonoBehaviour
{
    [System.Serializable]
    public class WeightedPrefab
    {
        public GameObject prefab;
        [Range(0f, 1f)]
        public float spawnChance = 1f;
    }

    [System.Serializable]
    public class SpawnGroup
    {
        public string groupName;

        [Header("Spawning Setup")]
        public Transform[] spawners;
        public List<WeightedPrefab> weightedPrefabs;

        [Header("Timing")]
        public bool useRandomInterval = false;
        public float spawnInterval = 2f;
        public float minInterval = 1f;
        public float maxInterval = 5f;

        private float timer;
        private float currentInterval;

        public void Init()
        {
            timer = 0f;
            currentInterval = useRandomInterval ? Random.Range(minInterval, maxInterval) : spawnInterval;
        }

        public void UpdateSpawning()
        {
            timer += Time.deltaTime;
            if (timer >= currentInterval)
            {
                timer = 0f;
                Spawn();

                currentInterval = useRandomInterval ? Random.Range(minInterval, maxInterval) : spawnInterval;
            }
        }

        private void Spawn()
        {
            if (spawners.Length == 0 || weightedPrefabs.Count == 0) return;

            Transform spawnPoint = spawners[Random.Range(0, spawners.Length)];
            GameObject selectedPrefab = GetRandomWeightedPrefab();

            if (selectedPrefab != null)
            {
                GameObject.Instantiate(selectedPrefab, spawnPoint.position, spawnPoint.rotation);
            }
        }

        private GameObject GetRandomWeightedPrefab()
        {
            float totalWeight = 0f;
            foreach (var wp in weightedPrefabs)
            {
                totalWeight += wp.spawnChance;
            }

            float randomValue = Random.value * totalWeight;
            float cumulativeWeight = 0f;

            foreach (var wp in weightedPrefabs)
            {
                cumulativeWeight += wp.spawnChance;
                if (randomValue <= cumulativeWeight)
                {
                    return wp.prefab;
                }
            }

            return null;
        }
    }

    public SpawnGroup streetGroup;
    public SpawnGroup roadsideGroup;
    public SpawnGroup sidewalkGroup;
    public SpawnGroup airGroup;

    void Start()
    {
        streetGroup.Init();
        roadsideGroup.Init();
        sidewalkGroup.Init();
        airGroup.Init();
    }

    void Update()
    {
        streetGroup.UpdateSpawning();
        roadsideGroup.UpdateSpawning();
        sidewalkGroup.UpdateSpawning();
        airGroup.UpdateSpawning();
    }
}
