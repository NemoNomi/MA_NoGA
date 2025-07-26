using UnityEngine;
using System.Collections;

///
/// Walks once to Waypoint A, turns 180°, then stops.
/// 

public class WalkAndTurn : MonoBehaviour
{
    [Header("Door Animation Settings")]
    [Tooltip("Animator that plays the 'door_anim_right' animation.")]
    [SerializeField] private Animator doorAnimator;

    [Tooltip("Name of the animation state to wait for.")]
    [SerializeField] private string doorAnimationName = "door_anim_right";

    [Tooltip("Delay in seconds after the animation ends.")]
    [SerializeField] private float delayAfterAnimation = 0.5f;

    [Header("Movement Settings")]
    [Tooltip("Target position to move to.")]
    [SerializeField] private Transform targetPosition;

    [Tooltip("Movement speed.")]
    [SerializeField] private float moveSpeed = 2f;

    private LookAtPlayerWhenIdle lookScript;
    private bool moving = false;
    private bool finished = false;

    void Start()
    {
        if (doorAnimator == null || targetPosition == null)
        {
            Debug.LogError("Missing references on MoveAfterDoor.");
            enabled = false;
            return;
        }

        lookScript = GetComponent<LookAtPlayerWhenIdle>();
        StartCoroutine(WaitForDoorAnimationAndMove());
    }

    IEnumerator WaitForDoorAnimationAndMove()
    {
        while (!doorAnimator.GetCurrentAnimatorStateInfo(0).IsName(doorAnimationName))
            yield return null;

        while (doorAnimator.GetCurrentAnimatorStateInfo(0).IsName(doorAnimationName) &&
               doorAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            yield return null;
        yield return new WaitForSeconds(delayAfterAnimation);

        if (lookScript) lookScript.enabled = false;
        moving = true;
    }

    void Update()
    {
        if (!moving || finished) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition.position,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPosition.position) < 0.01f)
        {
            transform.position = targetPosition.position;
            moving = false;
            finished = true;

            if (lookScript) lookScript.enabled = true;
        }
    }
}