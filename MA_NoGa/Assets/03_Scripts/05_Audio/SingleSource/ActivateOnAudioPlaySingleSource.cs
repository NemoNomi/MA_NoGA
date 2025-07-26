using UnityEngine;

///
/// Activates a GameObject when a specific AudioClip from any IAudioClipProvider starts playing.
///

public class ActivateOnAudioPlaySingleSource : MonoBehaviour
{
    #region Inspector

    [Tooltip("Script that provides AudioSource and AudioClips (must implement IAudioClipProvider).")]
    [SerializeField] private MonoBehaviour audioClipProviderSource;

    [Tooltip("Index of the AudioClip to monitor.")]
    [SerializeField] private int audioClipIndex = 0;

    [Tooltip("Object to activate.")]
    [SerializeField] private GameObject objectToActivate;

    [Header("Activation Options")]
    [Tooltip("Activate immediately when clip starts if true, otherwise after delay or when finished.")]
    [SerializeField] private bool activateAfterClip = false;

    [Tooltip("Optional delay after the clip starts before activating.")]
    [SerializeField] private float delayAfterStart = 0f;

    #endregion

    #region State

    private bool activated = false;
    private bool clipStarted = false;
    private float startTime;
    private IAudioClipProvider provider;

    #endregion

    #region Unity Methods
    private void Start()
    {
        provider = audioClipProviderSource as IAudioClipProvider;

        if (provider == null)
        {
            Debug.LogError("Assigned script does not implement IAudioClipProvider.");
            enabled = false;
            return;
        }

        if (objectToActivate == gameObject)
        {
            Debug.LogWarning("objectToActivate is this GameObject — disabling it will also disable this script.");
        }

        if (objectToActivate && objectToActivate != gameObject)
            objectToActivate.SetActive(false);
    }

    private void Update()
    {
        if (activated || provider.AudioSource == null)
            return;

        if (!clipStarted &&
            provider.AudioSource.isPlaying &&
            provider.AudioSource.clip == provider.AudioClips[audioClipIndex])
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
            if (!provider.AudioSource.isPlaying)
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
        if (objectToActivate)
            objectToActivate.SetActive(true);

        activated = true;
    }

    #endregion
}
