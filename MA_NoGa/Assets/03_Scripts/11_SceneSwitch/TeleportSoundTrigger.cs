using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

/// <summary>
/// Door-side trigger: registers TeleportSoundPlayer and plays the shared clip on each teleport.
/// Attach to every teleport door.
/// </summary>

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