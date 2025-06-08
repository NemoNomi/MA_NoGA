using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioOnInsert : MonoBehaviour
{
    public AudioClip insertClip;

    public bool waitUntilFinished = false;

    AudioSource src;

    void Awake()
    {
        src = GetComponent<AudioSource>();
        src.playOnAwake = false;
    }

    public IEnumerator Play()
    {
        if (insertClip == null) yield break;

        src.clip = insertClip;
        src.Play();

        if (waitUntilFinished)
            yield return new WaitForSeconds(insertClip.length);
    }
}
