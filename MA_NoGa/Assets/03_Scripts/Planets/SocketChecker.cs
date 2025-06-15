using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

/// <summary>
/// Detects when a specific object is inserted into this
/// XRSocketInteractor.
/// Once detected, it plays a TimedAudioOnInsert sequence,
/// fades the screen, and loads the next scene.
/// </summary>

[RequireComponent(typeof(XRSocketInteractor))]
public class SocketChecker : MonoBehaviour
{
    #region Inspector
    [Header("Expected Object")]
    [SerializeField] private GameObject keyObject;

    [Header("Audio")]
    [SerializeField] private TimedAudioOnInsert audioPlayer;
    #endregion

    #region Cached References
    private XRSocketInteractor socket;
    #endregion

    #region State
    private bool sequenceStarted;
    #endregion

    #region Unity Lifecycle
    private void Awake() => socket = GetComponent<XRSocketInteractor>();

    private void Update()
    {
        if (sequenceStarted || !socket.hasSelection) return;

        var selected = socket.interactablesSelected[0]?.transform.gameObject;
        if (selected == keyObject)
        {
            sequenceStarted = true;
            StartCoroutine(PlayAudioFadeAndLoad());
        }
    }
    #endregion

    #region Fade and Load Scene
    private IEnumerator PlayAudioFadeAndLoad()
    {
        if (audioPlayer)
            yield return StartCoroutine(audioPlayer.PlaySequence());

        var fader = FindFirstObjectByType<FadeScreenUniversal>();
        if (fader)
            yield return fader.FadeOut();

        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextIndex);
    }
    #endregion
}
