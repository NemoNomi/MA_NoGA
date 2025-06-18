using UnityEngine;

/// <summary>
/// Activates a XRGrabInteractable based on Audio-Events
/// </summary>
/// 
public class EnableGrabOnAudioPlay : MonoBehaviour
{
    #region Inspector
    [SerializeField] private MonoBehaviour timedAudioComponent;
    [SerializeField] private int audioIndex = 0;

    [Header("Grab-Target")]
    [SerializeField] private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabToEnable;

    [Header("Activation Options")]
    [SerializeField] private bool activateAfterClip = false;
    [SerializeField] private float delayAfterStart = 0f;
    #endregion

    #region State
    private AudioSource targetSource;
    private bool audioStarted;
    private bool activated;
    private float startTime;
    #endregion

    #region Unity-Events
    private void Start()
    {
        if (grabToEnable) grabToEnable.enabled = false;

        targetSource = ResolveTargetSource();
    }

    private void Update()
    {
        if (activated || targetSource == null) return;

        if (!audioStarted && targetSource.isPlaying)
        {
            audioStarted = true;
            startTime = Time.time;

            if (!activateAfterClip && delayAfterStart <= 0f)
            {
                Activate();
                return;
            }
        }

        if (!audioStarted) return;

        if (activateAfterClip)
        {
            if (!targetSource.isPlaying) Activate();
        }
        else if (Time.time - startTime >= delayAfterStart)
        {
            Activate();
        }
    }
    #endregion

    #region Helpers
    private void Activate()
    {
        if (grabToEnable) grabToEnable.enabled = true;
        activated = true;
    }

    private AudioSource ResolveTargetSource()
    {
        if (!timedAudioComponent) return null;

        AudioSource[] sources = timedAudioComponent switch
        {
            TimedAudioOnStart t => t.audioSources,
            TimedAudioOnGrab t => t.audioSources,
            TimedAudioOnTrigger t => t.audioSources,
            TimedAudioOnInsert t => t.audioSources,
            _ => null
        };

        return (sources != null &&
                audioIndex >= 0 &&
                audioIndex < sources.Length)
               ? sources[audioIndex]
               : null;
    }
    #endregion
}
