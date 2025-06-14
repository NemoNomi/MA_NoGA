using UnityEngine;

public class AnimationOnTriggerEnter : MonoBehaviour
{
    public string triggeringTag = "PlayerHand";
    public float requiredHoldTime = 5f;
    public Animator targetAnimator1;
    public Animator targetAnimator2;
    public string animationTriggerName = "HandInside";

    private float timer = 0f;
    private bool isInside = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggeringTag))
        {
            isInside = true;
            timer = 0f;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isInside && other.CompareTag(triggeringTag))
        {
            timer += Time.deltaTime;
            if (timer >= requiredHoldTime)
            {
                TriggerAnimations();
                isInside = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(triggeringTag))
        {
            isInside = false;
            timer = 0f;
        }
    }

    private void TriggerAnimations()
    {
        if (targetAnimator1 != null)
            targetAnimator1.SetTrigger(animationTriggerName);

        if (targetAnimator2 != null)
            targetAnimator2.SetTrigger(animationTriggerName);
    }
}
