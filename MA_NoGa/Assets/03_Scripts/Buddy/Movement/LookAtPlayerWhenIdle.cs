using UnityEngine;

/// <summary>
/// Rotates the whole Buddy (BuddyRoot) to face the camera when idle.
/// Only activates when WalkAndTurn reports no movement or turning.
/// </summary>

public class LookAtPlayerWhenIdle : MonoBehaviour
{
    public Transform cam;
    [SerializeField] private float rotationSpeed = 90f;

    void Start()
    {
        if (Camera.main != null)
            cam = Camera.main.transform;
        else
            Debug.LogWarning("No Main Camera found in scene.");
    }

void Update()
    {
        if (cam == null) return;

        Vector3 direction = cam.position - transform.position;
        direction.y = 0f;
        if (direction.sqrMagnitude < 0.001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}