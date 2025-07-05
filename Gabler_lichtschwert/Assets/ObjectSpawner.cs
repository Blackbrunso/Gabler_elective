using System.Collections;
using UnityEngine;

[System.Serializable]
public class SpawnableObject
{
    public GameObject prefab;
    [Range(0f, 1f)] public float spawnChance = 1f;
    public float initialSpeed = 5f;
}

public class ObjectSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public SpawnableObject[] spawnableObjects;
    public Transform[] spawnPoints;
    public float spawnInterval = 2f;

    private void Start()
    {
        if (spawnableObjects.Length == 0)
        {
            Debug.LogError("ObjectSpawner: Keine spawnbaren Objekte zugewiesen!");
            enabled = false;
            return;
        }

        if (spawnPoints.Length == 0)
        {
            Debug.LogError("ObjectSpawner: Keine Spawnpunkte zugewiesen!");
            enabled = false;
            return;
        }

        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnObject();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnObject()
    {
        // Zufälliger Spawnpunkt
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Zufälliges Objekt basierend auf Gewichtung
        SpawnableObject selected = GetRandomObject();
        if (selected == null || selected.prefab == null)
        {
            Debug.LogWarning("ObjectSpawner: Kein gültiges Objekt zum Spawnen gefunden.");
            return;
        }

        // Objekt instanziieren
        GameObject spawned = Instantiate(selected.prefab, spawnPoint.position, spawnPoint.rotation);

        // Bewegung zuweisen
        AutoMover mover = spawned.GetComponent<AutoMover>();
        if (mover != null)
        {
            mover.moveSpeed = selected.initialSpeed;
        }
        else
        {
            Debug.LogWarning($"ObjectSpawner: Prefab '{selected.prefab.name}' hat kein AutoMover-Skript.");
        }
    }

    private SpawnableObject GetRandomObject()
    {
        float totalWeight = 0f;

        foreach (var obj in spawnableObjects)
            totalWeight += obj.spawnChance;

        if (totalWeight <= 0f)
            return null;

        float randomValue = Random.Range(0f, totalWeight);
        float cumulative = 0f;

        foreach (var obj in spawnableObjects)
        {
            cumulative += obj.spawnChance;
            if (randomValue <= cumulative)
                return obj;
        }

        return null; // Fallback
    }
}
