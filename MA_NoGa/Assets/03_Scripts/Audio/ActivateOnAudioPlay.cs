using UnityEngine;

public class ActivateOnAudioPlay : MonoBehaviour
{
       public MonoBehaviour timedAudioComponent;

    [Tooltip("Zero-based index of the clip that will trigger activation.")]
    public int audioIndex = 0;

    [Tooltip("The GameObject to activate when that clip starts.")]
    public GameObject objectToActivate;

    private bool hasActivated = false;

    void Update()
    {
        if (hasActivated || timedAudioComponent == null) return;

        AudioSource[] sources = null;
        if (timedAudioComponent is TimedAudioOnStart tas)
            sources = tas.audioSources;
        else if (timedAudioComponent is TimedAudioOnGrab tai)
            sources = tai.audioSources;

        if (sources == null || sources.Length <= audioIndex) return;

        var src = sources[audioIndex];
        if (src != null && src.isPlaying)
        {
            objectToActivate.SetActive(true);
            hasActivated = true;
        }
    }
}
