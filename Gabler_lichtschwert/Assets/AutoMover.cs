using UnityEngine;

public class AutoMover : MonoBehaviour
{
    public float moveSpeed = 5f;

    public float lifetime = 10f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Bewegung nach vorne relativ zur Objektrotation
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
}
