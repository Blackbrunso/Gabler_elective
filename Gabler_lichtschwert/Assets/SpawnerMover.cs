using UnityEngine;

public class SpawnerMover : MonoBehaviour
{
    public float moveDistance = 0.5f;   // Sinus-Verschiebung auf Z-Achse
    public float moveSpeed = 1f;        // Sinus-Geschwindigkeit
    public float slideSpeed = 2f;       // Geschwindigkeit zum Einschieben

    public GameObject targetCenter;     // Zielobjekt für die Position

    private float startZ;               // Start Z außerhalb (z.B. Ziel.z - 10)
    private bool isSlidingIn = false;   // Flag, ob gerade eingeschoben wird
    private float randomOffset;
    private bool arrived = false;

    void Start()
    {
        if (targetCenter == null)
        {
            Debug.LogError("Bitte targetCenter zuweisen!");
            enabled = false;
            return;
        }

        // Startposition auf Z 10 Einheiten hinter dem Ziel
        startZ = targetCenter.transform.position.z - 10f;

        
       
        

        randomOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    void Update()
    {
        if (targetCenter == null) return;

        Vector3 pos = transform.position;
        Vector3 targetPos = targetCenter.transform.position;

        if (isSlidingIn)
        {
            // Langsam auf Ziel-Z fahren, X,Y bleiben gleich
            pos.z = Mathf.MoveTowards(pos.z, targetPos.z, slideSpeed * Time.deltaTime);
            transform.position = pos;

            if (Mathf.Abs(pos.z - targetPos.z) < 0.01f)
            {
                pos.z = targetPos.z;
                transform.position = pos;
                isSlidingIn = false;
                arrived = true;
            }
        }
        else if (arrived)
        {
            // Sinusbewegung auf Z um targetPos.z
            float offset = Mathf.Sin(Time.time * moveSpeed + randomOffset) * moveDistance;
            pos.z = targetPos.z + offset;
            transform.position = pos;
        }
    }

    // Aufruf wenn Auto gespawnt wurde und der Spawnpunkt reinrutschen soll
    public void SlideIn()
    {
        isSlidingIn = true;
    }
}
