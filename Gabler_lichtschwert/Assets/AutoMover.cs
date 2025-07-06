using UnityEngine;

public class AutoMover : MonoBehaviour
{
    public float moveSpeed = 5f;

    [Header("Lifetime Settings")]
    public bool useLifetime = true;
    public float lifetime = 10f;
    public int wert;

    private void Start()
    {
        if (useLifetime)
        {
            Destroy(gameObject, lifetime);
        }
    }

    void Update()
    {
        // Bewegung nach vorne relativ zur Objektrotation
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
}
