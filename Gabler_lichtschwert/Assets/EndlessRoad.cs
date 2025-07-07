using System.Collections.Generic;
using UnityEngine;

public class EndlessRoad : MonoBehaviour
{
    public GameObject[] roadPrefabs;         
    public Transform player;                  
    public float roadLength = 10f;            
    public int numberOfRoadsOnScreen = 5;     
    private float spawnZ = 0.0f;             
    private List<GameObject> activeRoads = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < numberOfRoadsOnScreen; i++)
        {
            SpawnRoad(i < 2 ? 0 : -1); 
        }
    }

    void Update()
    {
        
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
