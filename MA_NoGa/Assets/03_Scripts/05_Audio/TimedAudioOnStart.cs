using UnityEngine;
using System.Collections;

/// <summary>
/// Plays a list of AudioSource clips after the scene starts.
/// </summary>

public class TimedAudioOnStart : MonoBehaviour
{
    #region Inspector
    [Tooltip("One AudioSource per clip.")]
    public AudioSource[] audioSources;

    [Tooltip(
        "delays[0] -- pause after scene start before clip 0\n" +
        "delays[i] -- pause after clip (i-1) before clip i"
    )]
    [SerializeField] private float[] delays;
    #endregion

    private void Start()
    {
        if (audioSources.Length != delays.Length)
        {
            Debug.LogError("'audioSources' and 'delays' must have the same length.");
            return;
        }

        StartCoroutine(PlaySequence());
    }

    #region Coroutinr
    private IEnumerator PlaySequence()
    {
        yield return new WaitForSeconds(delays[0]);

        for (int i = 0; i < audioSources.Length; i++)
        {
            var src = audioSources[i];

            if (src && src.clip)
                src.Play();
            else
                Debug.LogWarning($"AudioSource[{i}] or its clip is null.");

            float clipLen = src && src.clip ? src.clip.length : 0f;
            float nextDelay = (i + 1 < delays.Length) ? delays[i + 1] : 0f;

            if (i + 1 < audioSources.Length)
                yield return new WaitForSeconds(clipLen + nextDelay);
        }
    }
    #endregion
}
