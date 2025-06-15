using UnityEngine;

/// <summary>
/// Activates a GameObject when a specified AudioSource starts playing.
/// </summary>
public class ActivateOnAudioPlay : MonoBehaviour
{
    #region Inspector
    [SerializeField] private MonoBehaviour timedAudioComponent;
    [SerializeField] private int audioIndex = 0;
    [SerializeField] private GameObject objectToActivate;
    #endregion

    #region State
    private AudioSource targetSource;
    private bool activated;
    #endregion

    #region Unity Events
    private void Start()
    {
        if (objectToActivate) objectToActivate.SetActive(false);
        targetSource = ResolveTargetSource();
    }

    private void Update()
    {
        if (activated || targetSource == null) return;

        if (targetSource.isPlaying)
        {
            objectToActivate.SetActive(true);
            activated = true;
        }
    }
    #endregion

    #region Helpers
    private AudioSource ResolveTargetSource()
    {
        if (timedAudioComponent == null) return null;

        AudioSource[] sources = timedAudioComponent switch
        {
            TimedAudioOnStart t => t.audioSources,
            TimedAudioOnGrab t => t.audioSources,
            TimedAudioOnTrigger t => t.audioSources,
            _ => null
        };

        return (sources != null && audioIndex < sources.Length) ? sources[audioIndex] : null;
    }
    #endregion
}
