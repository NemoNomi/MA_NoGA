using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

///
/// Loads the next scene when one or more PlayerHand colliders remain inside
/// this trigger for defined seconds (radial Progress).
///

[RequireComponent(typeof(Collider))]
public class SceneLoadOnTrigger : MonoBehaviour
{
    #region Inspector
    [Header("Timer")]
    [SerializeField] private float holdTime = 3f;

    [Header("Progress UI")]
    [SerializeField] private Image progressRing;
    #endregion

    #region State
    private Coroutine holdRoutine;
    private int handsInside;
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        if (progressRing) progressRing.fillAmount = 0f;
    }
    #endregion

    #region Trigger Callbacks
    private void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("PlayerHand")) return;

        if (++handsInside == 1)
            holdRoutine = StartCoroutine(HoldCountdown());
    }

    private void OnTriggerExit(Collider col)
    {
        if (!col.CompareTag("PlayerHand")) return;

        handsInside = Mathf.Max(0, handsInside - 1);
        if (handsInside == 0) ResetProgress();
    }
    #endregion

    #region Coroutine
    private IEnumerator HoldCountdown()
    {
        float t = 0f;

        while (t < holdTime)
        {
            if (progressRing) progressRing.fillAmount = t / holdTime;
            if (handsInside == 0) yield break;

            t += Time.deltaTime;
            yield return null;
        }

        if (progressRing) progressRing.fillAmount = 1f;

        var fader = FindFirstObjectByType<FadeScreenUniversal>();
        if (fader) yield return fader.FadeOut();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    #endregion

    #region Helpers
    private void ResetProgress()
    {
        if (holdRoutine != null) StopCoroutine(holdRoutine);

        holdRoutine = null;
        if (progressRing) progressRing.fillAmount = 0f;
    }
    #endregion
}
