using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WeaponShooter : MonoBehaviour
{
    #region Inspector
    public Transform spawnPoint;

    public float shootForce = 500f;

    [Header("Pooling")]
    public BallPooler ballPooler;

    [Header("Grab Interactable")]
    [Tooltip("XR Grab Interactable Component on Weapon.")]
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    #endregion

    void OnEnable()
    {
        if (grabInteractable != null)
            grabInteractable.activated.AddListener(OnActivated);
    }

    void OnDisable()
    {
        if (grabInteractable != null)
            grabInteractable.activated.RemoveListener(OnActivated);
    }

    private void OnActivated(ActivateEventArgs args)
    {
        if (args.interactorObject is UnityEngine.XR.Interaction.Toolkit.Interactors.XRDirectInteractor)
        {
            Shoot();
        }
    }



    #region Shoot
    public void Shoot()
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
    #endregion

    private System.Collections.IEnumerator DeactivateAfterTime(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }
}