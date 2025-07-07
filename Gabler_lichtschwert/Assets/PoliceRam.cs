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
        if (anim != null && rb.gameObject != gameObject && !rb.transform.IsChildOf(transform))
        {
            //anim.enabled = false;
        }


        if (rb.isKinematic)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }

        Vector3 direction = transform.forward * 0.8f + Vector3.up * 0.5f;

       
        float side = Random.Range(-1f, 1f); 
        direction += transform.right * side * 0.5f;

        direction.Normalize(); 

        rb.AddForce(direction * ramForce, ForceMode.Impulse);

    }
}
