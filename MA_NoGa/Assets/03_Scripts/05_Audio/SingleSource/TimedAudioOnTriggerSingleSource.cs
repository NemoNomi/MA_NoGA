using UnityEngine;
using System.Collections;

///
/// Plays a list of AudioClips through a single AudioSource
/// when a collider with the specified tag enters this trigger.
///

public class TimedAudioOnTriggerSingleSource : MonoBehaviour, IAudioClipProvider
{
    #region IAudioClipProvider Interface

    public AudioSource AudioSource => audioSource;
    public AudioClip[] AudioClips => audioClips;

    #endregion

    #region Inspector

    [Header("Audio Settings")]
    [Tooltip("Clips to play in sequence.")]
    [SerializeField] private AudioClip[] audioClips;

    [Tooltip("Single AudioSource used to play the clips.")]
    [SerializeField] private AudioSource audioSource;

    [Tooltip(
        "delays[0] -- pause after trigger before clip 0\n" +
        "delays[i] -- pause after clip (i-1) before clip i"
    )]
    [SerializeField] private float[] delays;

    [Header("Trigger Settings")]
    [Tooltip("Tag of the collider that can start the sequence.")]
    [SerializeField] private string triggeringTag = "Player";

    #endregion

    #region State

    private bool triggered;

    #endregion

    #region Trigger Callback

    private void OnTriggerEnter(Collider other)
    {
        if (triggered || !other.CompareTag(triggeringTag)) return;

        if (!audioSource)
        {
            Debug.LogError("No AudioSource assigned.");
            return;
        }

        if (audioClips.Length != delays.Length)
        {
            Debug.LogError("'audioClips' and 'delays' must have the same length.");
            return;
        }

        triggered = true;
        StartCoroutine(PlaySequence());
    }

    #endregion

    #region Coroutine

    private IEnumerator PlaySequence()
    {
        yield return new WaitForSeconds(delays[0]);

        for (int i = 0; i < audioClips.Length; i++)
        {
            AudioClip clip = audioClips[i];

            if (clip)
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
            else
            {
                Debug.LogWarning($"audioClips[{i}] is null.");
            }

            float clipLen = clip ? clip.length : 0f;
            float nextDelay = (i + 1 < delays.Length) ? delays[i + 1] : 0f;

            if (i + 1 < audioClips.Length)
                yield return new WaitForSeconds(clipLen + nextDelay);
        }
    }

    #endregion
}
