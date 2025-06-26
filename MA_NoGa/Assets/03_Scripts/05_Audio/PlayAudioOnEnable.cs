using UnityEngine;

/// Plays clip through `audioSource every time this GameObject is enabled,
/// except for the very first enable that happens when the scene starts.
public class PlayAudioOnEnable : MonoBehaviour
{
    [Header("Drag-ins")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip   clip;

    private bool _seenFirstEnable = false;

    private void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        if (!_seenFirstEnable)
        {
            _seenFirstEnable = true;
            return;
        }

        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
        else
            Debug.LogWarning($"{name}: AudioSource or Clip missing.", this);
    }
}
