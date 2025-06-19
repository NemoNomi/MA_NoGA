using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// Activates a XRGrabInteractable based on when a specific AudioClip is played by TimedAudioOnStartSingleSource.
/// </summary>

public class EnableGrabOnAudioPlaySingleSource : MonoBehaviour
{
    #region Inspector

    [SerializeField] private TimedAudioOnStartSingleSource timedAudioSource;
    [SerializeField] private int audioClipIndex = 0;

    [Header("Grab-Target")]
    [SerializeField] private XRGrabInteractable grabToEnable;

    [Header("Activation Options")]
    [SerializeField] private bool activateAfterClip = false;
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
        if (activated || timedAudioSource.audioSource == null)
            return;

        if (!clipStarted && timedAudioSource.audioSource.isPlaying &&
            timedAudioSource.audioSource.clip == timedAudioSource.audioClips[audioClipIndex])
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
            if (!timedAudioSource.audioSource.isPlaying)
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
