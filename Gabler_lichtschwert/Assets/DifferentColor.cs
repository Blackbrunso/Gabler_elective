using UnityEngine;

public class DifferentColor : MonoBehaviour
{
    public int materialIndex = 0;
    void Start()
    {
        SkinnedMeshRenderer smr = GetComponentInChildren<SkinnedMeshRenderer>();
        if (smr != null && smr.materials.Length > materialIndex)
        {
            Material[] mats = smr.materials; 
            mats[materialIndex] = new Material(mats[materialIndex]); 
            mats[materialIndex].color = new Color(Random.value, Random.value, Random.value);
            smr.materials = mats; 
        }
        else
        {
            Debug.LogWarning("MaterialIndex ist auﬂerhalb des Bereichs oder SkinnedMeshRenderer nicht gefunden.");
        }
    }
}
