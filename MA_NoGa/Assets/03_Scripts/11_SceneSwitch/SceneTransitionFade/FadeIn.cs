using System.Collections;
using UnityEngine;

///
/// Fades this renderer from fully opaque to transparent over
/// fadeDuration, then destroys the GameObject.
///

[RequireComponent(typeof(Renderer))]
public class FadeIn : MonoBehaviour
{
    #region Inspector
    [SerializeField] private float fadeDuration = 1f;
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

    private void Start() => StartCoroutine(FadeOut());
    #endregion

    #region Coroutine
    private IEnumerator FadeOut()
    {
        float t = 0f;
        float invDuration = 1f / Mathf.Max(fadeDuration, 0.01f);

        while (t < 1f)
        {
            t += Time.deltaTime * invDuration;
            SetAlpha(Mathf.Lerp(1f, 0f, t));
            yield return null;
        }

        Destroy(gameObject);
    }
    #endregion

    #region Helpers
    private void SetAlpha(float alpha)
    {
        if (!rend.sharedMaterial.HasProperty(baseColorID)) return;

        rend.GetPropertyBlock(mpb);

        var c = mpb.HasVector(baseColorID) ? mpb.GetColor(baseColorID) : Color.black;
        c.a = Mathf.Clamp01(alpha);

        mpb.SetColor(baseColorID, c);
        rend.SetPropertyBlock(mpb);
    }
    #endregion
}