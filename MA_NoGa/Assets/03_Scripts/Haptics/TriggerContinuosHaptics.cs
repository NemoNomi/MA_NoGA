using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

/// <summary>
/// Plays a repeating haptic impulse as long as a collider tagged with
/// triggering tag stays inside this trigger.  
/// Works with any controller that already has a HapticImpulsePlayer.
/// </summary>

[RequireComponent(typeof(Collider))]
public class TriggerContinuousHaptics : MonoBehaviour
{
    #region Inspector
    [Header("Tag")]
    [SerializeField] private string triggeringTag = "PlayerHand";

    [Header("Haptic Settings")]
    [Range(0f, 1f)] [SerializeField] private float amplitude = 0.6f;
    [SerializeField] private float duration  = 0.05f;
    [SerializeField] private float frequency = 0f;
    [SerializeField] private float interval  = 0.1f;
    #endregion

    #region State
    private HapticImpulsePlayer player;
    private Coroutine rumble;
    #endregion

    #region Trigger Callbacks
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(triggeringTag) || rumble != null)
            return;

        player = other.GetComponentInParent<HapticImpulsePlayer>(true);
        if (player != null)
            rumble = StartCoroutine(RumbleLoop());
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(triggeringTag))
            return;

        StopRumble();
    }
    #endregion

    #region Helpers
    private IEnumerator RumbleLoop()
    {
        while (true)
        {
            player.SendHapticImpulse(amplitude, duration, frequency);
            yield return new WaitForSeconds(interval);
        }
    }

    private void StopRumble()
    {
        if (rumble != null)
        {
            StopCoroutine(rumble);
            rumble = null;
        }

        player = null;
    }
    #endregion
}
