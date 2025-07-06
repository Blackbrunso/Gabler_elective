using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [System.Serializable]
    public class SpawnGroup
    {
        public string groupName;

        [Header("Spawning Setup")]
        public Transform[] spawners;
        public GameObject[] spawnPrefabs;

        [Header("Timing")]
        public bool useRandomInterval = false;
        public float spawnInterval = 2f;        // Festes Intervall
        public float minInterval = 1f;          // Für zufälliges Spawning
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

                // Nächstes Intervall festlegen
                currentInterval = useRandomInterval ? Random.Range(minInterval, maxInterval) : spawnInterval;
            }
        }

        private void Spawn()
        {
            if (spawners.Length == 0 || spawnPrefabs.Length == 0) return;

            Transform spawnPoint = spawners[Random.Range(0, spawners.Length)];
            GameObject prefab = spawnPrefabs[Random.Range(0, spawnPrefabs.Length)];

            GameObject.Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
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
