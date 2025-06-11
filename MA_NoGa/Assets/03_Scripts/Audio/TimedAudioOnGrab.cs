using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class TimedAudioOnGrab : MonoBehaviour
{
    [Tooltip("Die Objekte, die das Audio starten, wenn eines davon gegrabbt wird.")]
    public List<XRGrabInteractable> triggerObjects;

    [Tooltip("AudioSources, die in Sequenz abgespielt werden.")]
    public AudioSource[] audioSources;

    [Tooltip("Delays in Sekunden. delays[0] = Wartezeit nach dem Grab, delays[i] (i>0) = Wartezeit nach Clip[i-1].")]
    public float[] delays;

    private bool hasTriggered = false;

    void OnEnable()
    {
        foreach (var interactable in triggerObjects)
        {
            if (interactable != null)
                interactable.selectEntered.AddListener(OnGrab);
        }
    }

    void OnDisable()
    {
        foreach (var interactable in triggerObjects)
        {
            if (interactable != null)
                interactable.selectEntered.RemoveListener(OnGrab);
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (hasTriggered)
            return;

        hasTriggered = true;
        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        if (audioSources.Length != delays.Length)
        {
            Debug.LogError("TimedAudioOnGrab: 'audioSources' und 'delays' müssen gleich lang sein.");
            yield break;
        }

        yield return new WaitForSeconds(delays[0]);

        for (int i = 0; i < audioSources.Length; i++)
        {
            if (audioSources[i] != null)
                audioSources[i].Play();
            else
                Debug.LogWarning($"TimedAudioOnGrab: audioSources[{i}] ist null.");

            if (i + 1 < audioSources.Length)
            {
                float clipLen = audioSources[i].clip != null ? audioSources[i].clip.length : 0f;
                yield return new WaitForSeconds(clipLen + delays[i + 1]);
            }
        }
    }
}
