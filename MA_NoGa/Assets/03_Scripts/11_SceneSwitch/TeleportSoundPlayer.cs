using UnityEngine;

/// <summary>
/// Singleton audio player that survives scene loads; 
/// call EnsureReady(clip) once, then Play() on each teleport.
/// </summary>

[RequireComponent(typeof(AudioSource))]
public sealed class TeleportSoundPlayer : MonoBehaviour
{
    private static TeleportSoundPlayer _instance;
    private AudioSource _src;

    public static void EnsureReady(AudioClip clip)
    {
        if (_instance == null)
        {
            var go = new GameObject("_TeleportSoundPlayer");
            _instance = go.AddComponent<TeleportSoundPlayer>();
            DontDestroyOnLoad(go);

            _instance._src = go.GetComponent<AudioSource>();
            _instance._src.playOnAwake = false;
            _instance._src.loop = false;
            _instance._src.spatialBlend = 1f;
        }

        if (_instance._src.clip == null && clip != null)
            _instance._src.clip = clip;
    }

    public static void Play()
    {
        if (_instance == null || _instance._src.clip == null) return;

        _instance._src.Stop();
        _instance._src.Play();
    }
}