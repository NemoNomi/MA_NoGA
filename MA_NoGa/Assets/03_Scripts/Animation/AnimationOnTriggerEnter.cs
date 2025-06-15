using UnityEngine;

/// <summary>
/// Fires a trigger on one or more Animators after a collider
/// with the specified tag stays inside this trigger for a given duration.
/// </summary>

public class AnimationOnTriggerEnter : MonoBehaviour
{
    #region Inspector
    [SerializeField] private string triggeringTag = "PlayerHand";
    [SerializeField] private float requiredHoldTime = 5f;
    [SerializeField] private Animator[] targetAnimators = null;
    [SerializeField] private string animationTrigger = "HandInside";
    #endregion

    #region State
    private float timer;
    private bool isInside;
    #endregion

    #region Trigger Handling
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(triggeringTag)) return;

        isInside = true;
        timer = 0f;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isInside || !other.CompareTag(triggeringTag)) return;

        timer += Time.deltaTime;
        if (timer >= requiredHoldTime)
        {
            TriggerAnimations();
            ResetState();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(triggeringTag)) return;
        ResetState();
    }
    #endregion

    #region Animation
    private void TriggerAnimations()
    {
        foreach (var anim in targetAnimators)
            if (anim) anim.SetTrigger(animationTrigger);
    }
    #endregion

    #region Reset
    private void ResetState()
    {
        isInside = false;
        timer = 0f;
    }
    #endregion
}
