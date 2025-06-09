using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;


public class TimedAudioOnInsert : MonoBehaviour
{
    [Header("Socket Trigger")]
    [Tooltip("The XR Socket that, when filled, starts the audio sequence.")]
    public XRSocketInteractor socketInteractor;

    [Header("Audio Sequence")]
    [Tooltip("One AudioSource per clip. Can all live on this GameObject or on different ones.")]
    public AudioSource[] audioSources;

    [Tooltip(
        "Delays in seconds:\n" +
        "delays[0] = wait after insert before first clip;\n" +
        "delays[i>0] = wait after previous clip ends before next."
    )]
    public float[] delays;

    private bool hasTriggered = false;

    void OnEnable()
    {
        if (socketInteractor != null)
            socketInteractor.selectEntered.AddListener(OnSocketFilled);
    }

    void OnDisable()
    {
        if (socketInteractor != null)
            socketInteractor.selectEntered.RemoveListener(OnSocketFilled);
    }

    private void OnSocketFilled(SelectEnterEventArgs args)
    {
        if (hasTriggered) return;
        hasTriggered = true;
        StartCoroutine(PlaySequence());
    }

    /// <summary>
    /// Plays all audioSources in order, observing the delays,
    /// and ensuring the last clip fully finishes before returning.
    /// </summary>
    public IEnumerator PlaySequence()
    {
        if (audioSources.Length != delays.Length)
        {
            Debug.LogError("TimedAudioOnInsert: 'audioSources' and 'delays' must be the same length.");
            yield break;
        }

        yield return new WaitForSeconds(delays[0]);

        for (int i = 0; i < audioSources.Length; i++)
        {
            var src = audioSources[i];
            if (src != null && src.clip != null)
            {
                src.Play();
            }
            else
            {
                Debug.LogWarning($"TimedAudioOnInsert: audioSources[{i}] or its clip is null.");
            }

            float clipLen = src != null && src.clip != null ? src.clip.length : 0f;
            float extraDelay = (i + 1 < delays.Length) ? delays[i + 1] : 0f;
            yield return new WaitForSeconds(clipLen + extraDelay);
        }
    }
}
