using UnityEngine;

/// <summary>
///  The GameObject with this script is not Destroyed on Load
///  is a singleton,
///  is persistent over scene changes.
/// </summary>

[RequireComponent(typeof(AudioSource))]
public class PersistentBGM : MonoBehaviour
{
    private static PersistentBGM instance;
    private AudioSource src;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        src = GetComponent<AudioSource>();
        if (src != null && !src.isPlaying)
            src.Play();
    }
}
