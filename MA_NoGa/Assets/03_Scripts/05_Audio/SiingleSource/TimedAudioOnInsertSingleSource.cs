using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
public class TimedAudioOnInsertSingleSource : MonoBehaviour, IAudioClipProvider
{
    public AudioSource audioSource;
    public AudioClip[] audioClips;

    public AudioSource AudioSource => audioSource;
    public AudioClip[] AudioClips => audioClips;

    #region Inspector

    [Header("Socket Trigger")]
    [SerializeField] private XRSocketInteractor socketInteractor;

    [Header("Audio Setup")]

    [Tooltip(
        "delays[0] -- pause after insert, before the first clip\n" +
        "delays[i] -- pause after clip (i-1), before clip i"
    )]
    [SerializeField] private float[] delays;

    #endregion

    #region State

    private bool triggered;

    #endregion

    #region Unity Methods

    private void OnEnable() => ToggleListener(true);
    private void OnDisable() => ToggleListener(false);

    #endregion

    #region Socket Listener

    private void ToggleListener(bool add)
    {
        if (!socketInteractor) return;

        if (add) socketInteractor.selectEntered.AddListener(OnSocketFilled);
        else socketInteractor.selectEntered.RemoveListener(OnSocketFilled);
    }

    private void OnSocketFilled(SelectEnterEventArgs _)
    {
        if (triggered) return;
        triggered = true;

        if (audioClips.Length != delays.Length)
        {
            Debug.LogError("'audioClips' and 'delays' must have the same length.");
            return;
        }

        if (!audioSource)
        {
            Debug.LogError("No AudioSource assigned.");
            return;
        }

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

            yield return new WaitForSeconds(clipLen + nextDelay);
        }
    }

    #endregion
}