using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// Enables a XRGrabInteractable when a specific AudioClip from TimedAudioOnStartSingleSource is played.
/// </summary>
public class EnableGrabOnAudioPlaySingleSource : MonoBehaviour
{
    #region Inspector

    [SerializeField] private TimedAudioOnStartSingleSource timedAudioSource;
    [SerializeField] private int audioClipIndex = 0;

    [Header("Grab-Target")]
    [SerializeField] private XRGrabInteractable grabToEnable;

    [Header("Activation Options")]
    [Tooltip("Enable after clip finishes instead of when it starts.")]
    [SerializeField] private bool activateAfterClip = false;

    [Tooltip("Optional delay after the clip starts before enabling grab.")]
    [SerializeField] private float delayAfterStart = 0f;

    #endregion

    #region State

    private bool activated = false;
    private bool clipStarted = false;
    private float startTime;

    #endregion

    private void Start()
    {
        if (grabToEnable)
            grabToEnable.enabled = false;
    }

    private void Update()
    {
        if (activated || timedAudioSource.AudioSource == null)
            return;

        if (!clipStarted && timedAudioSource.AudioSource.isPlaying &&
            timedAudioSource.AudioSource.clip == timedAudioSource.AudioClips[audioClipIndex])
        {
            clipStarted = true;
            startTime = Time.time;

            if (!activateAfterClip && delayAfterStart <= 0f)
            {
                Activate();
                return;
            }
        }

        if (!clipStarted)
            return;

        if (activateAfterClip)
        {
            if (!timedAudioSource.AudioSource.isPlaying)
                Activate();
        }
        else
        {
            if (Time.time - startTime >= delayAfterStart)
                Activate();
        }
    }

    private void Activate()
    {
        if (grabToEnable)
            grabToEnable.enabled = true;

        activated = true;
    }
}
