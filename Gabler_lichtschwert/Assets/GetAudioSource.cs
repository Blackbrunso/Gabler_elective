using System.Linq;
using UnityEngine;

public class AudioGroup : MonoBehaviour
{
    public enum AudioType { SFX, Music }

    [System.Serializable]
    public class AudioEntry
    {
        public AudioSource source;
        public AudioType type;
    }

    public AudioEntry[] audioEntries;

    public AudioSource[] GetSFXSources()
    {
        return System.Array.FindAll(audioEntries, e => e.type == AudioType.SFX && e.source != null)
                           .Select(e => e.source).ToArray();
    }

    public AudioSource[] GetMusicSources()
    {
        return System.Array.FindAll(audioEntries, e => e.type == AudioType.Music && e.source != null)
                           .Select(e => e.source).ToArray();
    }
}
