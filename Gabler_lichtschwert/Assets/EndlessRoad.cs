using System.Collections.Generic;
using UnityEngine;

public class EndlessRoad : MonoBehaviour
{
    public GameObject[] roadPrefabs;          // Deine 5 Road-Prefabs
    public Transform player;                  // Die Position, die sich vorwärts bewegt (z. B. Kamera oder Spieler)
    public float roadLength = 10f;            // Die Länge eines Road-Prefabs
    public int numberOfRoadsOnScreen = 5;     // Wie viele Road-Chunks sichtbar bleiben
    private float spawnZ = 0.0f;              // Wo der nächste Road-Chunk gespawnt wird
    private List<GameObject> activeRoads = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < numberOfRoadsOnScreen; i++)
        {
            SpawnRoad(i < 2 ? 0 : -1); // Die ersten paar sind immer gleich, danach random
        }
    }

    void Update()
    {
        // Sobald der Player nahe genug am nächsten Spawnpunkt ist, eine neue Straße hinzufügen
        if (player.position.z - 20 > spawnZ - numberOfRoadsOnScreen * roadLength)
        {
            SpawnRoad();
            DeleteOldestRoad();
        }
    }

    void SpawnRoad(int prefabIndex = -1)
    {
        GameObject go;
        if (prefabIndex == -1)
            prefabIndex = Random.Range(0, roadPrefabs.Length);

        go = Instantiate(roadPrefabs[prefabIndex], Vector3.forward * spawnZ, Quaternion.identity);
        activeRoads.Add(go);
        spawnZ += roadLength;
    }

    void DeleteOldestRoad()
    {
        Destroy(activeRoads[0]);
        activeRoads.RemoveAt(0);
    }
}
