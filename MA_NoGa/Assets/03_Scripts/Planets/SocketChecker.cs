using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRSocketInteractor))]
public class SocketSceneLoader : MonoBehaviour
{
    [Header("Erwartetes Objekt")]
    public GameObject keyObject;

    XRSocketInteractor socket;
    bool fulfilled;

    void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();
    }

    void Update()
    {
        if (fulfilled || !socket.hasSelection) return;

        var interactable = socket.interactablesSelected[0];
        if (interactable == null) return;

        if (interactable.transform.gameObject == keyObject)
        {
            fulfilled = true;
            StartCoroutine(FadeThenLoad());
        }
    }

    IEnumerator FadeThenLoad()
    {
var fader = GameObject.FindAnyObjectByType<FadeScreenUniversal>();
        if (fader != null)
            yield return fader.FadeOut();

        int next = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(next);
    }
}
