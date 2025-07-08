using UnityEngine;

/// <summary>
/// Walks once to Waypoint X and then stops.
/// Starts Walking when Player Enters trigger of the GameObject this script lays on.
/// </summary>
/// 
/// 
[RequireComponent(typeof(Collider))]
public class WalkToTargetOnTrigger : MonoBehaviour
{
    #region Inspector
    [SerializeField] private Transform character;
    [SerializeField] private Transform targetPosition;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private string triggeringTag = "Player";
    #endregion

    #region State
    private LookAtPlayerWhenIdle lookScript;
    private bool moving;
    private bool finished;
    #endregion

    #region Setup
    private void Awake()
    {
        if (!character) character = transform.parent ?? transform;
        lookScript = character.GetComponent<LookAtPlayerWhenIdle>();
        GetComponent<Collider>().isTrigger = true;
    }
    #endregion

    #region Trigger
    private void OnTriggerEnter(Collider other)
    {
        if (moving || finished || !other.CompareTag(triggeringTag)) return;
        if (lookScript) lookScript.enabled = false;
        moving = true;
    }
    #endregion

    #region Movement
    private void Update()
    {
        if (!moving || finished || !targetPosition) return;

        character.position = Vector3.MoveTowards(
            character.position,
            targetPosition.position,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(character.position, targetPosition.position) < 0.01f)
        {
            character.position = targetPosition.position;
            moving = false;
            finished = true;
            if (lookScript) lookScript.enabled = true;
        }
    }
    #endregion
}