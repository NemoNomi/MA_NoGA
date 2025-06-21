using UnityEngine;

/// <summary>
/// Rotates this GameObject to face the target camera
/// but only when the object is idle — i.e., not moving or turning via the WalkAndTurn script.
/// </summary>
public class LookAtPlayerWhenIdle : MonoBehaviour
{
    #region Fields
    public Transform targetCamera;
    private WalkAndTurn walkAndTurn;
    #endregion

    #region Unity Methods
    void Start()
    {
        if (targetCamera == null)
        {
            Camera mainCam = Camera.main;
            if (mainCam != null) targetCamera = mainCam.transform;
        }

        walkAndTurn = GetComponent<WalkAndTurn>();
    }

    void Update()
    {
        if (walkAndTurn != null && !walkAndTurn.IsMoving && !walkAndTurn.IsTurning)
        {
            Vector3 direction = targetCamera.position - transform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation, lookRotation, 90f * Time.deltaTime);
            }
        }
    }
    #endregion
}
