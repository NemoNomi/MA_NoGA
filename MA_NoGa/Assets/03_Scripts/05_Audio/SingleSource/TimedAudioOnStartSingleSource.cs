using UnityEngine;
using System.Collections;

///
/// Plays a list of AudioClips from one single AudioSource after scene starts.
///

public class TimedAudioOnStartSingleSource : MonoBehaviour, IAudioClipProvider
{
    #region Inspector

    [Tooltip("Single AudioSource to play all clips.")]
    [SerializeField] private AudioSource audioSource;

    [Tooltip("Array of clips to play sequentially.")]
    [SerializeField] private AudioClip[] audioClips;

    [Tooltip(
        "delays[0] -- pause after scene start before clip 0\n" +
        "delays[i] -- pause after clip (i-1) before clip i"
    )]
    [SerializeField] private float[] delays;

    #endregion

    #region Interface Implementation

    public AudioSource AudioSource => audioSource;
    public AudioClip[] AudioClips => audioClips;

    #endregion

    private void Start()
    {
        if (audioClips.Length != delays.Length)
        {
            Debug.LogError("'audioClips' and 'delays' must have the same length.");
            return;
        }

        StartCoroutine(PlaySequence());
    }

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
                yield return new WaitForSeconds(clip.length);
            }
            else
            {
                Debug.LogWarning($"AudioClip[{i}] is null.");
            }

            if (i + 1 < delays.Length)
            {
                yield return new WaitForSeconds(delays[i + 1]);
            }
        }
    }

    #endregion
}
