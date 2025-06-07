using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
[RequireComponent(typeof(Collider))]
public class SceneLoadOnTrigger : MonoBehaviour
{
    public float holdTime = 3f;
    public int sceneBuildIndex = 2;

    [Header("Progress UI")]
    [Tooltip("Ring-Image, dessen FillAmount hochgezählt wird.")]
    public Image progressRing;

    Coroutine holdRoutine;
    int handsInside = 0;

    void Awake()
    {
        if (progressRing) progressRing.fillAmount = 0f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (IsHand(other))
        {
            handsInside++;
            if (handsInside == 1)
                holdRoutine = StartCoroutine(HoldCountdown());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (IsHand(other))
        {
            handsInside = Mathf.Max(0, handsInside - 1);
            if (handsInside == 0)
                ResetProgress();
        }
    }

    bool IsHand(Collider col) => col.CompareTag("PlayerHand");

    IEnumerator HoldCountdown()
    {
        float t = 0f;

        while (t < holdTime)
        {
            if (progressRing)
                progressRing.fillAmount = t / holdTime;

            t += Time.deltaTime;
            yield return null;
        }

        if (progressRing) progressRing.fillAmount = 1f;
        SceneManager.LoadScene(sceneBuildIndex);
    }

    void ResetProgress()
    {
        if (holdRoutine != null)
        {
            StopCoroutine(holdRoutine);
            holdRoutine = null;
        }
        if (progressRing) progressRing.fillAmount = 0f;
    }
}
