using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class SceneLoadOnTrigger : MonoBehaviour
{
    [Header("Timer & Zielszene")]
    public float holdTime = 3f;
    public int   sceneBuildIndex = 2;

    [Header("Progress UI")]
    public Image progressRing;

    Coroutine holdRoutine;
    int handsInside = 0;

    void Awake()
    {
        if (progressRing != null)
            progressRing.fillAmount = 0f;
    }

    void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("PlayerHand")) return;

        if (++handsInside == 1)
            holdRoutine = StartCoroutine(HoldCountdown());
    }

    void OnTriggerExit(Collider col)
    {
        if (!col.CompareTag("PlayerHand")) return;

        handsInside = Mathf.Max(0, handsInside - 1);
        if (handsInside == 0) ResetProgress();
    }

    IEnumerator HoldCountdown()
    {
        float t = 0f;
        while (t < holdTime)
        {
            if (progressRing != null)
                progressRing.fillAmount = t / holdTime;

            if (handsInside == 0) yield break;
            t += Time.deltaTime;
            yield return null;
        }

        if (progressRing != null)
            progressRing.fillAmount = 1f;

        var fader = GameObject.FindFirstObjectByType<FadeScreenUniversal>();
        if (fader != null)
            yield return fader.FadeOut();

        SceneManager.LoadScene(sceneBuildIndex);
    }

    void ResetProgress()
    {
        if (holdRoutine != null)
            StopCoroutine(holdRoutine);

        holdRoutine = null;

        if (progressRing != null)
            progressRing.fillAmount = 0f;
    }
}
