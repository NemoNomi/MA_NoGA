using UnityEngine;
using System.Collections;

public class TimedAudioOnStart : MonoBehaviour
{
    [Tooltip("One AudioSource per clip. Can all live on this GameObject or on different ones.")]
    public AudioSource[] audioSources;

    [Tooltip("Delays in seconds. delays[0] = wait after scene start before first clip. delays[i] (i>0) = wait after clip[i-1] ends before playing clip[i].")]
    public float[] delays;

    void Start()
    {
        if (audioSources.Length != delays.Length)
        {
            Debug.LogError("TimedAudioPlayer: 'audioSources' and 'delays' must have the same length.");
            return;
        }
        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        yield return new WaitForSeconds(delays[0]);

        for (int i = 0; i < audioSources.Length; i++)
        {
            if (audioSources[i] != null)
                audioSources[i].Play();
            else
                Debug.LogWarning($"TimedAudioPlayer: audioSources[{i}] is null.");

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
