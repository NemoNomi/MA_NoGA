using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRSocketInteractor))]
public class SocketChecker : MonoBehaviour
{
    [Header("Erwartetes Objekt")]
    [Tooltip("Das Objekt, das in den Socket gesteckt werden muss.")]
    public GameObject keyObject;

    [Header("Audio")]
    [Tooltip("Dein TimedAudioOnInsert–Component, der die komplette Sequenz abspielt.")]
    public TimedAudioOnInsert audioPlayer;

    private XRSocketInteractor socket;
    private bool sequenceStarted = false;

    void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();
    }

    void Update()
    {
        // Nur einmal auslösen und nur, wenn tatsächlich etwas selektiert ist
        if (sequenceStarted || !socket.hasSelection) return;

        var selected = socket.interactablesSelected[0]?.transform.gameObject;
        if (selected == keyObject)
        {
            sequenceStarted = true;
            StartCoroutine(PlayAudioThenFadeAndLoad());
        }
    }

    private IEnumerator PlayAudioThenFadeAndLoad()
    {
        // 1) Warte auf das komplette Abspielen aller Clips (mit ihren Delays)
        if (audioPlayer != null)
            yield return StartCoroutine(audioPlayer.PlaySequence());

        // 2) Fade-Out
        var fader = FindObjectOfType<FadeScreenUniversal>();
        if (fader != null)
            yield return fader.FadeOut();

        // 3) Nächste Szene laden
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextIndex);
    }
}
