using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public float lifeTime = 10f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
