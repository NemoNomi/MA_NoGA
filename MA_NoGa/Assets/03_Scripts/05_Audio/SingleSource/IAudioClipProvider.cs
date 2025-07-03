using UnityEngine;

public interface IAudioClipProvider
{
    AudioSource AudioSource { get; }
    AudioClip[] AudioClips { get; }
}
