using UnityEngine;

public class GetCaught : MonoBehaviour
{
    [Header("Spawning")]
    public GameObject policePrefab;            
    public Transform[] spawnPoints;           

    

    [Header("Limit Settings")]
    public int maxSpawns = 3;                  

    private int policeSpawnCount = 0;          

    
    public bool busted;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Police")) return;

        if (policeSpawnCount >= maxSpawns)
        {
            busted = true;
            Debug.Log("Zu oft erwischt! Game Over oder andere Aktion.");
            return;
        }

        if (policePrefab != null && policeSpawnCount < spawnPoints.Length && spawnPoints[policeSpawnCount] != null)
        {
            Transform spawnTransform = spawnPoints[policeSpawnCount];

            GameObject clone = Instantiate(policePrefab, spawnTransform.position, spawnTransform.rotation);
            clone.transform.SetParent(spawnTransform);

            SpawnerMover mover = spawnTransform.GetComponent<SpawnerMover>();
            if (mover != null)
            {
                mover.SlideIn();
            }

            policeSpawnCount++;
        }


        else
        {
            Debug.LogWarning("PolicePrefab oder SpawnPoint fehlt!");
        }
    }
}
