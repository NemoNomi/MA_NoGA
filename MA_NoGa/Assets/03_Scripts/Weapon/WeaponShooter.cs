using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WeaponShooter : MonoBehaviour
{
    [Tooltip("Ort, an dem das Projektil erscheint.")]
    public Transform spawnPoint;

    [Tooltip("Wie stark das Projektil weggeschossen wird.")]
    public float shootForce = 500f;

    [Header("Pooling")]
    public BallPooler ballPooler;

    [Header("Grab Interactable")]
    [Tooltip("XR Grab Interactable Component an der Waffe.")]
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    private bool hasRecentlyShot = false;

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


    private void ResetShotFlag()
    {
        hasRecentlyShot = false;
    }

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

    private System.Collections.IEnumerator DeactivateAfterTime(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }
}