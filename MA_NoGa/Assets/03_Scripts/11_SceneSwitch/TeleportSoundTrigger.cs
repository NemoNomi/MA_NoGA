using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

///
/// Door-side trigger: registers TeleportSoundPlayer and plays the clip on each teleport.
///

[RequireComponent(typeof(TeleportationArea))]
public sealed class TeleportSoundTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip teleportClip;

    private TeleportationArea _area;

    private void Awake()
    {
        _area = GetComponent<TeleportationArea>();
        TeleportSoundPlayer.EnsureReady(teleportClip);
        _area.teleporting.AddListener(OnTeleporting);
    }

    private void OnTeleporting(TeleportingEventArgs _) => TeleportSoundPlayer.Play();

    private void OnDestroy() => _area?.teleporting.RemoveListener(OnTeleporting);
}