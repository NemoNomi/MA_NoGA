using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

///
/// Toggles Sound on VideoPlayer with Toggle UI.
/// 

public class VideoSoundToggle : MonoBehaviour
{
    #region Inspector
    [SerializeField] private Toggle toggle;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private VideoPlayer videoPlayer;
    #endregion

    #region Unity Messages
    private void Awake()
    {
        if (toggle == null) toggle = GetComponent<Toggle>();

        toggle.onValueChanged.AddListener(SetVolume);
        SetVolume(toggle.isOn);
    }

    private void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(SetVolume);
    }
    #endregion

    #region Volume Setting
    private void SetVolume(bool on)
    {
        if (audioSource) audioSource.volume = on ? 1f : 0f;
        if (videoPlayer) videoPlayer.SetDirectAudioMute(0, !on);
    }
    #endregion
}
