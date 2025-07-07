using System.Collections;
using UnityEngine;
using EzySlice;
using UnityEngine.AI;
using NUnit.Framework.Internal;
using TMPro;

public class ObjectSlicer : MonoBehaviour
{

    // hier das Tutorial auf dem der code basiert
    // https://www.youtube.com/watch?v=GQzW6ZJFQ94&t=847s

    [Header("Slicing Settings")]
    public Transform startSlicePoint;
    public Transform endSlicePoint;
    public Material cutMaterial;
    public LayerMask sliceableLayer;
    [Range(0, 10000)]
    public float explosionForce = 2000f;
    public float destroyAfterSeconds = 10f;
    public int point;
    private bool sliceReady = true;
    private Vector3 lastSlicePos = Vector3.zero;
    private Vector3 sliceVelocity;
    private SaberAudio sA;
    public GameObject add;

    private void Start()
    {
        sA = GetComponent<SaberAudio>();
    }
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
        if (sliceReady)
        {
            lastSlicePos = endSlicePoint.position;
            sliceReady = false;
            return;
        }
        else
        {
            sliceVelocity = endSlicePoint.position - lastSlicePos;
            sliceReady = true;
        }

        if (target.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
        {
            agent.enabled = false;
            Destroy(agent);
        }

        if (target.TryGetComponent<Animator>(out Animator animator))
        {
            animator.enabled = false;
        }

        Transform meshChild = target.GetComponentInChildren<SkinnedMeshRenderer>()?.transform ??
                              target.GetComponentInChildren<MeshFilter>()?.transform;

        if (meshChild == null) return;

        Mesh meshToSlice = null;
        Material[] materials = null;

        if (meshChild.TryGetComponent<SkinnedMeshRenderer>(out var skinned))
        {
            meshToSlice = BakeSkinnedMesh(skinned);
            materials = skinned.sharedMaterials;
        }
        else if (meshChild.TryGetComponent<MeshFilter>(out var mf))
        {
            meshToSlice = mf.sharedMesh;
            materials = meshChild.GetComponent<MeshRenderer>()?.sharedMaterials;
        }

        if (meshToSlice == null) return;

        Vector3 localPos = meshChild.localPosition;
        Quaternion localRot = meshChild.localRotation;
        Vector3 localScale = meshChild.localScale;

        GameObject temp = new GameObject("TempSliceObject");
        temp.transform.position = meshChild.position;
        temp.transform.rotation = meshChild.rotation;
        temp.transform.localScale = meshChild.lossyScale;

        var tempMF = temp.AddComponent<MeshFilter>();
        var tempMR = temp.AddComponent<MeshRenderer>();
        tempMF.sharedMesh = meshToSlice;
        tempMR.sharedMaterials = materials;

        Vector3 sliceNormal = Vector3.Cross(endSlicePoint.position - startSlicePoint.position, sliceVelocity).normalized;
        SlicedHull hull = temp.Slice(endSlicePoint.position, sliceNormal, cutMaterial);

        if (hull != null)
        {
            GameObject upper = hull.CreateUpperHull(temp, cutMaterial);
            GameObject lower = hull.CreateLowerHull(temp, cutMaterial);

            CopyAccurateTransform(meshChild.transform, upper.transform);
            CopyAccurateTransform(meshChild.transform, lower.transform);

            upper.transform.SetParent(target.transform.parent, true);
            lower.transform.SetParent(target.transform.parent, true);


            SetupSlicedPiece(upper);
            SetupSlicedPiece(lower);
        }

        if (target.GetComponent<AutoMover>() != null)
        {
            AutoMover punkte = target.GetComponent<AutoMover>();
            Points(punkte.wert);

            // --- Instantiate 'add' UI ---
            GameObject instance = Instantiate(add, target.transform.position, Quaternion.identity);

            TextMeshPro textComponent = instance.GetComponent<TextMeshPro>();


            if (textComponent != null)
            {
                textComponent.text = "+" + punkte.wert.ToString();
            }
            else
            {
                Debug.LogWarning("Kein TextMeshPro auf dem Prefab gefunden!");
            }



        }

        sA.PlayRandomClip();

        Destroy(temp);
        Destroy(target);
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
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        MeshCollider mc = piece.AddComponent<MeshCollider>();
        mc.sharedMesh = piece.GetComponent<MeshFilter>().sharedMesh;
        mc.convex = true;
        mc.inflateMesh = true;

        rb.AddExplosionForce(explosionForce, piece.transform.position, 1f);
        Destroy(piece, destroyAfterSeconds);
    }

    void CopyAccurateTransform(Transform source, Transform destination)
    {
        destination.position = source.position;
        destination.rotation = source.rotation;
        destination.localScale = source.lossyScale;
    }
    public void Points(int x)
    {
        point += x;
    }

}
