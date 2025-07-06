using UnityEngine;

public class PoliceRam : MonoBehaviour
{
    [Header("Ram-Kraft")]
    public float ramForce = 500f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Police")) return;

        Rigidbody rb = collision.rigidbody != null
            ? collision.rigidbody
            : collision.gameObject.GetComponentInParent<Rigidbody>();

        if (rb == null) return;

        MeshCollider meshCol = rb.GetComponentInChildren<MeshCollider>();
        if (meshCol != null && !meshCol.convex)
        {
            meshCol.convex = true;
        }

        Animator anim = rb.GetComponent<Animator>();
        if (anim != null)
        {
            anim.enabled = false;
        }

        if (rb.isKinematic)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }

        // Kraft-Richtung berechnen: schräg nach oben + zufällige seitliche Richtung
        Vector3 direction = transform.forward * 0.8f + Vector3.up * 0.5f;

        // Zufällige links/rechts-Komponente hinzufügen
        float side = Random.Range(-1f, 1f); // -1 = links, 1 = rechts
        direction += transform.right * side * 0.5f;

        direction.Normalize(); // sicherstellen, dass die Richtung eine Länge von 1 hat

        // Kraft anwenden
        rb.AddForce(direction * ramForce, ForceMode.Impulse);

    }
}
