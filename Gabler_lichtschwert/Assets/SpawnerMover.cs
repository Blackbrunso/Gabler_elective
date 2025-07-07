using UnityEngine;

public class SpawnerMover : MonoBehaviour
{
    public float moveDistance = 0.5f;
    public float moveSpeed = 1f;
    public float slideSpeed = 2f;
    public GameObject targetCenter;

    private float startZ;
    private bool isSlidingIn = false;
    private float randomOffset;
    private bool arrived = false;
    private float slideStartZ;

    void Start()
    {
        if (targetCenter == null)
        {
            enabled = false;
            return;
        }

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
            float offset = Mathf.Sin(Time.time * moveSpeed + randomOffset) * moveDistance;
            pos.z = targetPos.z + offset;
            transform.position = pos;
        }
    }

    public void SlideIn()
    {
        isSlidingIn = true;
        slideStartZ = transform.position.z;
    }
}
