using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioDistanceVolume : MonoBehaviour
{
    public Transform target;
    public float minDistance = 0f;
    public float maxDistance = 5f;
    public float minVolume = 0f;
    public float maxVolume = 1f;

    public bool randomizeSettings = false;
    public float distanceVariation = 1f;
    public float volumeVariation = 0.2f;

    private AudioSource audioSource;
    private bool hasChildSpawned = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = 0f;

        if (randomizeSettings)
        {
            maxDistance += Random.Range(-distanceVariation, distanceVariation);
            maxDistance = Mathf.Max(minDistance + 0.1f, maxDistance);

            maxVolume += Random.Range(-volumeVariation, volumeVariation);
            maxVolume = Mathf.Clamp01(maxVolume);
        }
    }

    void Update()
    {
        if (!hasChildSpawned && transform.childCount > 0)
        {
            hasChildSpawned = true;
            if (!audioSource.isPlaying)
                audioSource.Play();
        }

        if (!hasChildSpawned || target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);
        float t = Mathf.InverseLerp(maxDistance, minDistance, distance);
        float volume = Mathf.Lerp(minVolume, maxVolume, t);
        audioSource.volume = volume;
    }
}
