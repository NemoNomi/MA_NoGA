using UnityEngine;

///
/// Fires a trigger on one or more Animators after a collider
/// with the specified tag stays inside this trigger for specific duration.
/// Play a sound at the same moment the animation trigger is sent.
///

public class AnimationOnTriggerEnter : MonoBehaviour
{
    #region Inspector
    [Header("Trigger settings")]
    [SerializeField] private string triggeringTag = "PlayerHand";
    [SerializeField] private float requiredHoldTime = 5f;

    [Header("Animation")]
    [SerializeField] private Animator[] targetAnimators = null;
    [SerializeField] private string animationTrigger = "HandInside";

    [Header("Audio (optional)")]
    [SerializeField] private AudioSource audioSource = null;
    [SerializeField] private AudioClip clipToPlay = null;
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

    #region Animation + Sound
    private void TriggerAnimations()
    {
        foreach (var anim in targetAnimators)
            if (anim) anim.SetTrigger(animationTrigger);

        PlaySound();
    }

    private void PlaySound()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource != null && clipToPlay != null)
        {
            audioSource.PlayOneShot(clipToPlay);
        }
        else
        {
            if (clipToPlay == null)
                Debug.LogWarning($"{name}: No audio clip assigned.", this);
            if (audioSource == null)
                Debug.LogWarning($"{name}: No AudioSource found or assigned.", this);
        }
    }
    #endregion

    #region Helpers
    private void ResetState()
    {
        isInside = false;
        timer = 0f;
    }
    #endregion
}
