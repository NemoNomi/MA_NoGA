using UnityEngine;
using System.Collections;

/// <summary>
/// Walks once to Waypoint A, turns 180°, then stops.
/// </summary>
public class WalkAndTurn : MonoBehaviour
{
    public Animator sourceAnimator;
    public string triggerState = "Intro";
    public float bufferTime = 0.5f;

    [Header("Movement")]
    public Transform waypointA;
    public float moveSpeed = 4f;
    public float turnSpeed = 120f;
    public float stopDist = 0.05f;

    bool moving = false;
    bool turning = false;
    Quaternion finalRot;

    void Start()
    {
        StartCoroutine(WaitForTriggerAndGo());
    }

    IEnumerator WaitForTriggerAndGo()
    {
        while (!sourceAnimator.GetCurrentAnimatorStateInfo(0).IsName(triggerState))
            yield return null;

        while (sourceAnimator.GetCurrentAnimatorStateInfo(0).IsName(triggerState) &&
               sourceAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            yield return null;

        yield return new WaitForSeconds(bufferTime);

        moving = true;
    }

    void Update()
    {
        if (moving)
            MoveToWaypoint();
        else if (turning)
            TurnAround();
    }

    void MoveToWaypoint()
    {
        Vector3 dir = waypointA.position - transform.position;

        if (dir.sqrMagnitude > 0.0001f)
        {
            Quaternion look = Quaternion.LookRotation(dir.normalized);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, look, turnSpeed * Time.deltaTime);
        }

        transform.position = Vector3.MoveTowards(
            transform.position, waypointA.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, waypointA.position) <= stopDist)
        {
            moving = false;
            turning = true;
            finalRot = Quaternion.Euler(
                0f, transform.eulerAngles.y + 180f, 0f);
        }
    }

    void TurnAround()
    {
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation, finalRot, turnSpeed * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, finalRot) < 0.1f)
            turning = false;
    }
}