using System.Collections;
using UnityEngine;
using EzySlice;
using UnityEngine.AI;

public class ObjectSlicer : MonoBehaviour
{
    // hier das Main Tutorial
    // https://www.youtube.com/watch?v=GQzW6ZJFQ94&t=847s

    [Header("Slicing Settings")]
    public Transform startSlicePoint;
    public Transform endSlicePoint;
    public Material cutMaterial;
    public LayerMask sliceableLayer;
    [Range(0, 10000)]
    public float explosionForce = 2000f;
    public float destroyAfterSeconds = 10f;

    private bool sliceReady = true;
    private Vector3 lastSlicePos = Vector3.zero;
    private Vector3 sliceVelocity;

    void FixedUpdate()
    {
        if (Physics.Linecast(startSlicePoint.position, endSlicePoint.position, out RaycastHit hit, sliceableLayer))
        {
            GameObject target = FindRootWithRigidbodyOrCollider(hit.transform);
            if (target != null)
            {
                Slice(target);
            }
        }
    }

    void Slice(GameObject target)
    {
        // Berechne Slicegeschwindigkeit
        if (sliceReady)
        {
            lastSlicePos = endSlicePoint.position;
            sliceReady = false;
        }
        else
        {
            sliceVelocity = endSlicePoint.position - lastSlicePos;
            sliceReady = true;
        }

        // Deaktiviere NavMesh oder Animation
        if (target.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
        {
            agent.enabled = false;
            Destroy(agent);
        }
        if (target.TryGetComponent<Animator>(out Animator animator))
        {
            animator.enabled = false;
        }

        // Mesh vorbereiten
        Mesh meshToSlice = null;
        Material[] materials = null;

        SkinnedMeshRenderer skinned = target.GetComponentInChildren<SkinnedMeshRenderer>();
        if (skinned != null)
        {
            meshToSlice = BakeSkinnedMesh(skinned);
            materials = skinned.sharedMaterials;
        }
        else if (target.GetComponentInChildren<MeshFilter>() != null)
        {
            MeshFilter mf = target.GetComponentInChildren<MeshFilter>();
            meshToSlice = mf.sharedMesh;

            MeshRenderer mr = mf.GetComponent<MeshRenderer>();
            if (mr != null) materials = mr.sharedMaterials;
        }

        if (meshToSlice == null) return;

        // Temporäres Objekt zum Schneiden erstellen
        GameObject temp = new GameObject("TempSliceObject");
        temp.transform.SetPositionAndRotation(target.transform.position, target.transform.rotation);
        temp.transform.localScale = target.transform.lossyScale;

        MeshFilter tempMF = temp.AddComponent<MeshFilter>();
        MeshRenderer tempMR = temp.AddComponent<MeshRenderer>();
        tempMF.sharedMesh = meshToSlice;
        tempMR.sharedMaterials = materials;

        Vector3 sliceNormal = Vector3.Cross(endSlicePoint.position - startSlicePoint.position, sliceVelocity).normalized;
        SlicedHull hull = temp.Slice(endSlicePoint.position, sliceNormal, cutMaterial);

        if (hull != null)
        {
            GameObject upper = hull.CreateUpperHull(temp, cutMaterial);
            GameObject lower = hull.CreateLowerHull(temp, cutMaterial);

            CopyTransform(temp.transform, upper.transform);
            CopyTransform(temp.transform, lower.transform);

            SetupSlicedPiece(upper);
            SetupSlicedPiece(lower);
        }

        Destroy(temp);
        Destroy(target.transform.root.gameObject);
    }

    GameObject FindRootWithRigidbodyOrCollider(Transform t)
    {
        while (t != null)
        {
            if (t.GetComponent<Rigidbody>() != null || t.GetComponent<Collider>() != null)
                return t.gameObject;
            t = t.parent;
        }
        return null;
    }

    Mesh BakeSkinnedMesh(SkinnedMeshRenderer skinned)
    {
        Mesh mesh = new Mesh();
        skinned.BakeMesh(mesh);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        return mesh;
    }

    void SetupSlicedPiece(GameObject piece)
    {
        Rigidbody rb = piece.AddComponent<Rigidbody>();
        MeshCollider mc = piece.AddComponent<MeshCollider>();
        mc.sharedMesh = piece.GetComponent<MeshFilter>().sharedMesh;
        mc.convex = true;

        rb.AddExplosionForce(explosionForce, piece.transform.position, 1f);
        Destroy(piece, destroyAfterSeconds);
    }

    void CopyTransform(Transform source, Transform destination)
    {
        destination.position = source.position;
        destination.rotation = source.rotation;
        destination.localScale = source.lossyScale;
    }
}
