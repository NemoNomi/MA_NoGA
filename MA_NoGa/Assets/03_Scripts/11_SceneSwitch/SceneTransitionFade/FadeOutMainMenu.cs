using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

///
/// Fades a fullscreen quad from transparent to fadeColor over
/// fadeDuration, then loads the requested scene asynchronously and
/// activates it once ready.
///

[RequireComponent(typeof(Renderer))]
public class FadeOutMainMenu : MonoBehaviour
{
    #region Inspector
    [Header("Quad Renderer")]
    [SerializeField] private Renderer quadRenderer;

    [Header("Fade Settings")]
    [SerializeField, Range(0.1f, 5f)] private float fadeDuration = 1f;
    [SerializeField] private Color fadeColor = Color.black;
    #endregion

    #region Cached Data
    private Material quadMat;
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        quadRenderer ??= GetComponent<Renderer>();
        quadMat = quadRenderer.material;
        SetAlpha(0f);
    }
    #endregion

    #region Public API
    public void FadeAndLoad(int buildIndex) =>
        StartCoroutine(FadeOutThenLoad(buildIndex));
    #endregion

    #region Coroutine
    private IEnumerator FadeOutThenLoad(int buildIndex)
    {
        float t = 0f;
        float invDuration = 1f / Mathf.Max(fadeDuration, 0.01f);

        while (t < 1f)
        {
            t += Time.deltaTime * invDuration;
            SetAlpha(t);
            yield return null;
        }
        SetAlpha(1f);

        var op = SceneManager.LoadSceneAsync(buildIndex);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f) yield return null;
        op.allowSceneActivation = true;
    }
    #endregion

    #region Helpers
    private void SetAlpha(float alpha)
    {
        if (!quadMat.HasProperty("_BaseColor")) return;

        var c = fadeColor;
        c.a = Mathf.Clamp01(alpha);
        quadMat.SetColor("_BaseColor", c);
    }
    #endregion
}