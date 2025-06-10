using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponShooter : MonoBehaviour
{
    [Tooltip("Ort, an dem das Projektil erscheint.")]
    public Transform spawnPoint;

    [Tooltip("Wie stark das Projektil weggeschossen wird.")]
    public float shootForce = 500f;

    [Header("Input Action References")]
    [Tooltip("Input Action für die rechte Hand (z.B. TriggerButton RightHand XR Controller).")]
    public InputActionReference rightTriggerAction;

    [Tooltip("Input Action für die linke Hand (z.B. TriggerButton LeftHand XR Controller).")]
    public InputActionReference leftTriggerAction;

    [Header("Pooling")]
    public BallPooler ballPooler;

    void OnEnable()
    {
        if (rightTriggerAction != null)
        {
            rightTriggerAction.action.Enable();
            rightTriggerAction.action.performed += OnShoot;
        }
        if (leftTriggerAction != null)
        {
            leftTriggerAction.action.Enable();
            leftTriggerAction.action.performed += OnShoot;
        }
    }

    void OnDisable()
    {
        if (rightTriggerAction != null)
        {
            rightTriggerAction.action.performed -= OnShoot;
            rightTriggerAction.action.Disable();
        }
        if (leftTriggerAction != null)
        {
            leftTriggerAction.action.performed -= OnShoot;
            leftTriggerAction.action.Disable();
        }
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        Shoot();
    }

    void Shoot()
    {
        if (ballPooler == null || spawnPoint == null) return;

        GameObject projectile = ballPooler.GetPooledObject();
        if (projectile != null)
        {
            projectile.transform.position = spawnPoint.position;
            projectile.transform.rotation = spawnPoint.rotation;
            projectile.SetActive(true);

            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.AddForce(spawnPoint.forward * shootForce);
            }

            StartCoroutine(DeactivateAfterTime(projectile, 5f));
        }
    }

    private System.Collections.IEnumerator DeactivateAfterTime(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }
}
