using UnityEngine;

///
/// Activates a GameObject based on an AudioSource:
/// immediately when playback starts,
/// X seconds after playback starts,
/// or when the clip finishes.
///

public class ActivateOnAudioPlay : MonoBehaviour
{
    #region Inspector
    [SerializeField] private MonoBehaviour timedAudioComponent;
    [SerializeField] private int audioIndex = 0;
    [SerializeField] private GameObject objectToActivate;
    [Header("Activation Options")]
    [Tooltip("If true, wait until the clip ends. If false, use delayAfterStart.")]
    [SerializeField] private bool activateAfterClip = false;
    [Tooltip("Seconds after clip starts before activation (ignored if activateAfterClip = true).")]
    [SerializeField] private float delayAfterStart = 0f;
    #endregion

    #region State
    private AudioSource targetSource;
    private bool audioStarted;
    private bool activated;
    private float startTime;
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
            if (!targetSource.isPlaying)
                Activate();
        }
        else
        {
            if (Time.time - startTime >= delayAfterStart)
                Activate();
        }
    }
    #endregion

    #region Helpers
    private void Activate()
    {
        objectToActivate.SetActive(true);
        activated = true;
    }

    private AudioSource ResolveTargetSource()
    {
        if (timedAudioComponent == null) return null;

        AudioSource[] sources = timedAudioComponent switch
        {
            TimedAudioOnStart t => t.audioSources,
            TimedAudioOnGrab t => t.audioSources,
            TimedAudioOnTrigger t => t.audioSources,
            TimedAudioOnInsert t => t.audioSources,
            _ => null
        };


        return (sources != null && audioIndex >= 0 && audioIndex < sources.Length)
               ? sources[audioIndex]
               : null;
    }
    #endregion
}
