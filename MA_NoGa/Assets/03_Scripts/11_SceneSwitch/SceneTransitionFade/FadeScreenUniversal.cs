using System.Collections;
using UnityEngine;

/// <summary>
/// Universal fade-in / fade-out screen quad.  
/// Fades from opaque -- transparent on Start. 
/// Calls FadeOut to fade back to opaque and wait until done.  
/// </summary>

[RequireComponent(typeof(Renderer))]
public class FadeScreenUniversal : MonoBehaviour
{
    #region Inspector
    [Header("Durations (seconds)")]
    [SerializeField] private float fadeInDuration = 1f;
    [SerializeField] private float fadeOutDuration = 1f;

    [Header("Color")]
    [SerializeField] private Color fadeColor = Color.black;
    #endregion

    #region Cached Data
    private Renderer rend;
    private MaterialPropertyBlock mpb;
    private static readonly int baseColorID = Shader.PropertyToID("_BaseColor");
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        rend = GetComponent<Renderer>();
        mpb = new MaterialPropertyBlock();
        SetAlpha(1f);
    }

    private void Start() => StartCoroutine(Fade(1f, 0f, fadeInDuration));
    #endregion

    #region Public API
    public IEnumerator FadeOut() =>
        Fade(0f, 1f, fadeOutDuration);
    #endregion

    #region Coroutine
    private IEnumerator Fade(float from, float to, float duration)
    {
        float t = 0f;
        float invDuration = 1f / Mathf.Max(duration, 0.01f);

        while (t < 1f)
        {
            t += Time.deltaTime * invDuration;
            SetAlpha(Mathf.Lerp(from, to, t));
            yield return null;
        }
        SetAlpha(to);
    }
    #endregion

    #region Helpers
    private void SetAlpha(float alpha)
    {
        rend.GetPropertyBlock(mpb);

        var c = fadeColor;
        c.a = Mathf.Clamp01(alpha);
        mpb.SetColor(baseColorID, c);

        rend.SetPropertyBlock(mpb);
    }
    #endregion
}
