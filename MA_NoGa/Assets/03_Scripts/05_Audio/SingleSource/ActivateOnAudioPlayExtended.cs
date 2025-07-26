using UnityEngine;

///
/// Activates or deactivates Gameobjects or plays Animation,
/// based on Audioclips coming from IAudioClipProvider component.
///

public class ActivateOnAudioPlayExtended : MonoBehaviour
{
    #region Inspector

    [Header("Audiosource")]
    [SerializeField] private MonoBehaviour audioClipProviderSource;

    [SerializeField] private int audioClipIndex = 0;

    [Header("Objects")]
    [SerializeField] private GameObject[] objectsToActivate;

    [SerializeField] private GameObject[] objectsToDeactivate;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    [SerializeField] private string animationTrigger;

    [Header("Time Options")]
    [SerializeField] private bool afterClipEnd = false;

    [SerializeField] private float delayAfterStart = 0f;

    #endregion

    #region State

    private bool triggered = false;
    private bool clipStarted = false;
    private float clipStartTime;
    private IAudioClipProvider provider;

    #endregion

    #region Unity

    private void Start()
    {
        provider = audioClipProviderSource as IAudioClipProvider;

        if (provider == null)
        {
            Debug.LogError("Assigned script does not implement IAudioClipProvider.");
            enabled = false;
            return;
        }

        foreach (var go in objectsToActivate)
        {
            if (go != null && go != gameObject)
                go.SetActive(false);
        }
    }

    private void Update()
    {
        if (triggered || provider.AudioSource == null)
            return;

        var source = provider.AudioSource;
        var expectedClip = provider.AudioClips.Length > audioClipIndex ? provider.AudioClips[audioClipIndex] : null;

        if (expectedClip == null)
        {
            Debug.LogWarning($"AudioClip with index {audioClipIndex} not there.");
            return;
        }

        if (!clipStarted && source.isPlaying && source.clip == expectedClip)
        {
            clipStarted = true;
            clipStartTime = Time.time;

            if (!afterClipEnd && delayAfterStart <= 0f)
            {
                TriggerActions();
                return;
            }
        }

        if (!clipStarted)
            return;

        if (afterClipEnd)
        {
            if (!source.isPlaying)
                TriggerActions();
        }
        else
        {
            if (Time.time - clipStartTime >= delayAfterStart)
                TriggerActions();
        }
    }

    #endregion

    #region Actions

    private void TriggerActions()
    {
        triggered = true;

        foreach (var go in objectsToActivate)
        {
            if (go != null)
                go.SetActive(true);
        }

        foreach (var go in objectsToDeactivate)
        {
            if (go != null)
                go.SetActive(false);
        }

        if (animator != null && !string.IsNullOrEmpty(animationTrigger))
        {
            animator.SetTrigger(animationTrigger);
        }
    }

    #endregion
}
