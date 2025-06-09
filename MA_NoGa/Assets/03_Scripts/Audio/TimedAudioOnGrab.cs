using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class TimedAudioOnGrab : MonoBehaviour
{
    [Tooltip("The interactable object the player must grab to start the sequence")]
    public XRGrabInteractable triggerObject;

    [Tooltip("AudioSources to play in sequence")]
    public AudioSource[] audioSources;

    [Tooltip("Delays in seconds. delays[0] = wait after grab before first clip. delays[i] (i>0) = wait after clip[i-1] ends before playing clip[i].")]
    public float[] delays;

    bool hasTriggered = false;

    void OnEnable()
    {
        if (triggerObject != null)
            triggerObject.selectEntered.AddListener(OnGrab);
    }

    void OnDisable()
    {
        if (triggerObject != null)
            triggerObject.selectEntered.RemoveListener(OnGrab);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (hasTriggered) return;
        hasTriggered = true;
        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        if (audioSources.Length != delays.Length)
        {
            Debug.LogError("TimedAudioOnGrab: 'audioSources' and 'delays' arrays must be the same length.");
            yield break;
        }

        // 1) initial delay after grab
        yield return new WaitForSeconds(delays[0]);

        for (int i = 0; i < audioSources.Length; i++)
        {
            // 2) play this clip
            if (audioSources[i] != null)
                audioSources[i].Play();
            else
                Debug.LogWarning($"TimedAudioOnGrab: audioSources[{i}] is null.");

            // 3) if there’s a next clip, wait its length + its delay before looping
            if (i + 1 < audioSources.Length)
            {
                float clipLen = audioSources[i].clip != null
                    ? audioSources[i].clip.length
                    : 0f;
                yield return new WaitForSeconds(clipLen + delays[i + 1]);
            }
        }
    }
}
