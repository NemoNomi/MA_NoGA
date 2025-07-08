/*
 * VideoSoundToggle
 * ----------------
 * Stellt den Ton eines Video-Players per UI-Toggle an oder aus.
 * • Der Toggle schaltet entweder die Lautstärke der verknüpften AudioSource
 *   (0 ↔ 1) oder – optional – den Audiotrack des VideoPlayers.
 * • Hängt das Script an ein beliebiges Objekt (z. B. direkt an den Toggle)
 *   und weise im Inspector die Referenzen zu.
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoSoundToggle : MonoBehaviour
{
    #region Inspector
    [SerializeField] private Toggle       toggle;       // UI-Toggle
    [SerializeField] private AudioSource  audioSource;  // AudioSource des VideoPlayers
    [SerializeField] private VideoPlayer  videoPlayer;  // optional, falls direkt muten gewünscht
    #endregion

    #region Unity Messages
    private void Awake()
    {
        if (toggle == null) toggle = GetComponent<Toggle>();

        toggle.onValueChanged.AddListener(SetVolume);
        SetVolume(toggle.isOn);            // Anfangszustand anwenden
    }

    private void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(SetVolume);
    }
    #endregion

    #region Core
    private void SetVolume(bool on)
    {
        if (audioSource)  audioSource.volume = on ? 1f : 0f;

        // Optional: Audiotrack des VideoPlayers stummschalten
        if (videoPlayer)  videoPlayer.SetDirectAudioMute(0, !on);  // Track-Index 0
    }
    #endregion
}
