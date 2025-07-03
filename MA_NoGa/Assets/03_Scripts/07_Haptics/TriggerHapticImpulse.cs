using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;


/// <summary>
/// Plays one haptic impulse when collider tagged with
/// triggering tag enters this trigger.  
/// Works with any controller that already has a HapticImpulsePlayer.
/// </summary>

[RequireComponent(typeof(Collider))]
public class TriggerHapticImpulse : MonoBehaviour
{
    [Header("Tag")]
    [SerializeField] string triggeringTag = "PlayerHand";

    [Header("Haptic Impulse")]
    [Range(0f, 1f)]  [SerializeField] float amplitude = 0.6f;
    [Range(0f, 1f)]  [SerializeField] float duration  = 0.08f;
    [SerializeField] float frequency = 0f;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(triggeringTag)) return;

        var player = other.GetComponentInParent<HapticImpulsePlayer>(true);
        player?.SendHapticImpulse(amplitude, duration, frequency);
    }
}
