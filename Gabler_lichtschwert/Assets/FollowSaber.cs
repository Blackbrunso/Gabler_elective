using UnityEngine;

public class FollowSaber : MonoBehaviour
{
    public GameObject saber;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = saber.transform.position;
    }
}
