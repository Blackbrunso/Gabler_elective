using UnityEngine;

public class PoliceSiren : MonoBehaviour
{
    public Material blue;
    public Material red;
    public float interval = 0.5f;

    private SkinnedMeshRenderer renderer;
    private float timer;
    private bool isBlueFirst = true;

    void Start()
    {
        renderer = GetComponentInChildren<SkinnedMeshRenderer>();

        if (renderer == null)
        {
            Debug.LogWarning("Kein SkinnedMeshRenderer gefunden!");
        }
    }

    void Update()
    {
        if (renderer == null) return;

        timer += Time.deltaTime;

        if (timer >= interval)
        {
            Material[] mats = renderer.materials;

            if (mats.Length >= 2)
            {
                if (isBlueFirst)
                {
                    mats[8] = blue;
                    mats[9] = red;
                }
                else
                {
                    mats[8] = red;
                    mats[9] = blue;
                }

                renderer.materials = mats;
                isBlueFirst = !isBlueFirst;
            }

            timer = 0f;
        }
    }
}
