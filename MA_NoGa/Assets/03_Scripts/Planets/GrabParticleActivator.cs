using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRGrabInteractable))]
public class GrabParticleActivator : MonoBehaviour
{
    public ParticleSystem particles;

    void Awake()
    {
        if (particles == null)
            particles = GetComponentInChildren<ParticleSystem>(true);

        var grab = GetComponent<XRGrabInteractable>();
        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs _)
    {
        if (particles != null)
            particles.Play();
    }

    void OnRelease(SelectExitEventArgs _)
    {
        if (particles != null)
            particles.Stop();
    }
}
