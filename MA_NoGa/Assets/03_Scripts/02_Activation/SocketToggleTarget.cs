using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Enables the assigned target GameObject whenever an XR-grabbable 
/// is placed in this XRSocketInteractor and disables it when removed.
/// </summary>

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor))]
public class SocketToggleTarget : MonoBehaviour
{
    #region Fields
    [SerializeField] private GameObject target;
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socket;
    #endregion

    #region Unity
    private void Awake()
    {
        socket = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();
    }

    private void OnEnable()
    {
        socket.selectEntered.AddListener(OnSelectEntered);
        socket.selectExited.AddListener(OnSelectExited);

        target.SetActive(socket.hasSelection);
        if (socket.hasSelection)
        {
            ResumeAudio();
            ResumeParticles();
        }
    }

    private void OnDisable()
    {
        socket.selectEntered.RemoveListener(OnSelectEntered);
        socket.selectExited.RemoveListener(OnSelectExited);
    }
    #endregion

    #region Socket Callbacks
    private void OnSelectEntered(SelectEnterEventArgs _)
    {
        target.SetActive(true);
        ResumeAudio();
        ResumeParticles();
    }

    private void OnSelectExited(SelectExitEventArgs _)
    {
        PauseAudio();
        PauseParticles();
        target.SetActive(false);
    }
    #endregion

    #region Audio Helpers
    private void PauseAudio()
    {
        foreach (var src in target.GetComponentsInChildren<AudioSource>())
            if (src.isPlaying) src.Pause();
    }

    private void ResumeAudio()
    {
        foreach (var src in target.GetComponentsInChildren<AudioSource>())
            if (!src.isPlaying) src.UnPause();
    }
    #endregion

    #region Particle Helpers
    private void PauseParticles()
    {
        foreach (var ps in target.GetComponentsInChildren<ParticleSystem>())
            ps.Pause();
    }

    private void ResumeParticles()
    {
        foreach (var ps in target.GetComponentsInChildren<ParticleSystem>())
            ps.Play();
    }
    #endregion
}
