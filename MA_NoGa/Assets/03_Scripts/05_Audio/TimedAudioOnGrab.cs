using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// Plays a sequence of AudioSource clips when any given
/// <XRGrabInteractable is first grabbed.  
/// </summary>

public class TimedAudioOnGrab : MonoBehaviour
{
    #region Inspector
    [Header("Trigger")]
    [SerializeField] private List<XRGrabInteractable> triggerObjects;

    [Header("Audio")]
public AudioSource[] audioSources;
    

    [Tooltip("delay[0] = wait after grab, delays[i] (i>0) = wait after clip i-1")]
    [SerializeField] private float[] delays;
    #endregion

    #region State
    private bool triggered;
    #endregion

    private void OnEnable() => ToggleListeners(true);
    private void OnDisable() => ToggleListeners(false);

    #region Listener Helpers
    private void ToggleListeners(bool add)
    {
        foreach (var interactable in triggerObjects)
            if (interactable != null)
            {
                if (add) interactable.selectEntered.AddListener(OnGrab);
                else interactable.selectEntered.RemoveListener(OnGrab);
            }
    }
    #endregion

    #region Grab Callback
    private void OnGrab(SelectEnterEventArgs _)
    {
        if (triggered) return;
        triggered = true;

        if (audioSources.Length != delays.Length)
            Debug.LogError("TimedAudioOnGrab: 'audioSources' and 'delays' must have the same length.");
        else
            StartCoroutine(PlaySequence());
    }
    #endregion

    #region Coroutine
    private IEnumerator PlaySequence()
    {
        yield return new WaitForSeconds(delays[0]);

        for (int i = 0; i < audioSources.Length; i++)
        {
            var src = audioSources[i];

            if (src) src.Play();
            else Debug.LogWarning($"TimedAudioOnGrab: audioSources[{i}] is null.");

            if (i + 1 < audioSources.Length)
            {
                float clipLen = src && src.clip ? src.clip.length : 0f;
                yield return new WaitForSeconds(clipLen + delays[i + 1]);
            }
        }
    }
    #endregion
}
