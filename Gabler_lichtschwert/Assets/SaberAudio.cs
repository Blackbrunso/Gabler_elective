using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SaberAudio : MonoBehaviour
{
    public float speedThreshold = 2f;
    public GameObject saberTipObject;
    public GameObject saberRootObject;

    public AudioSource externalAudioSource;            // Ziel-AudioSource für zufällige Sounds
    public AudioClip[] randomClips;                    // Liste der möglichen Sounds

    private AudioSource audioSource;
    private Vector3 lastLocalPosition;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;

        if (saberTipObject != null && saberRootObject != null)
        {
            lastLocalPosition = saberRootObject.transform.InverseTransformPoint(saberTipObject.transform.position);
        }
        else
        {
            lastLocalPosition = Vector3.zero;
        }
    }

    void Update()
    {
        if (saberTipObject == null || saberRootObject == null)
        {
            return;
        }

        Vector3 currentLocalPosition = saberRootObject.transform.InverseTransformPoint(saberTipObject.transform.position);
        float speed = (currentLocalPosition - lastLocalPosition).magnitude / Time.deltaTime;

        if (speed > speedThreshold)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        }

        lastLocalPosition = currentLocalPosition;
    }

   
    public void PlayRandomClip()
    {
        if (externalAudioSource == null || randomClips == null || randomClips.Length == 0)
            return;

        AudioClip randomClip = randomClips[Random.Range(0, randomClips.Length)];
        externalAudioSource.clip = randomClip;
        externalAudioSource.Play();
    }
}
