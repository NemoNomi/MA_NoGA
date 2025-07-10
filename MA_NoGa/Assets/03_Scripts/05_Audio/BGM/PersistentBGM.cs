using UnityEngine;
using UnityEngine.SceneManagement;

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

    [Header("Volumes")]
    [Range(0f, 1f)]
    [SerializeField] private float volumeScene0 = 1f;

    [Range(0f, 1f)]
    [SerializeField] private float volumeOtherScenes = 0.5f;

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

        ApplyVolume(SceneManager.GetActiveScene());
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) => ApplyVolume(scene);

    private void ApplyVolume(Scene scene)
    {
        if (src == null) return;

        src.volume = scene.buildIndex == 0 ? volumeScene0 : volumeOtherScenes;
    }
}
