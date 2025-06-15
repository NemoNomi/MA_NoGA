using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

/// <summary>
/// Starts a ParticleSystem when this object is grabbed and
/// stops it when released.
/// </summary>

[RequireComponent(typeof(XRGrabInteractable))]
public class GrabParticleActivator : MonoBehaviour
{
    #region Inspector
    [SerializeField] private ParticleSystem particles;
    #endregion

    #region Cached References
    private XRGrabInteractable grab;
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        grab      = GetComponent<XRGrabInteractable>();
        particles = particles ? particles : GetComponentInChildren<ParticleSystem>(true);
    }

    private void OnEnable()  => ToggleListener(true);
    private void OnDisable() => ToggleListener(false);
    #endregion

    #region Listener
    private void ToggleListener(bool add)
    {
        if (add)
        {
            grab.selectEntered.AddListener(OnGrab);
            grab.selectExited .AddListener(OnRelease);
        }
        else
        {
            grab.selectEntered.RemoveListener(OnGrab);
            grab.selectExited .RemoveListener(OnRelease);
        }
    }

    private void OnGrab(SelectEnterEventArgs _) => particles?.Play();
    private void OnRelease(SelectExitEventArgs _) => particles?.Stop();
    #endregion
}
