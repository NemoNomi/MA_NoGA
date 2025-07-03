using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

/// <summary>
/// Plays a sequence of AudioSource clips once an
/// XRSocketInteractorbecomes occupied.  
/// </summary>

public class TimedAudioOnInsert : MonoBehaviour
{
    #region Inspector
    [Header("Socket Trigger")]
    [SerializeField] private XRSocketInteractor socketInteractor;

    [Header("Audio Sequence")]
    [Tooltip("One AudioSource per clip.")]
    public AudioSource[] audioSources;

    [Tooltip(
        "delays[0] -- pause after insert, before the first clip\n" +
        "delays[i] -- pause after clip (i-1), before clip i"
    )]
    [SerializeField] private float[] delays;
    #endregion

    #region State
    private bool triggered;
    #endregion

    private void OnEnable() => ToggleListener(true);
    private void OnDisable() => ToggleListener(false);

    #region Listener
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

        if (audioSources.Length != delays.Length)
            Debug.LogError("'audioSources' and 'delays' must have the same length.");
        else
            StartCoroutine(PlaySequence());
    }
    #endregion

    #region Coroutine
    public IEnumerator PlaySequence()
    {
        yield return new WaitForSeconds(delays[0]);

        for (int i = 0; i < audioSources.Length; i++)
        {
            var src = audioSources[i];

            if (src && src.clip) src.Play();
            else Debug.LogWarning($"audioSources[{i}] or its clip is null");

            float clipLen = src && src.clip ? src.clip.length : 0f;
            float nextDelay = (i + 1 < delays.Length) ? delays[i + 1] : 0f;

            yield return new WaitForSeconds(clipLen + nextDelay);
        }
    }
    #endregion
}
