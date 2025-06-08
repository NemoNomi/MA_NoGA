using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Renderer))]
public class FadeScreenUniversal : MonoBehaviour
{
    [Header("Dauer (s)")]
    public float fadeInDuration  = 1.0f;
    public float fadeOutDuration = 1.0f;

    [Header("Farbe")]
    public Color fadeColor = Color.black;

    Renderer rend;
    MaterialPropertyBlock mpb;
    static readonly int id = Shader.PropertyToID("_BaseColor");

    void Awake()
    {
        rend = GetComponent<Renderer>();
        mpb  = new MaterialPropertyBlock();
        SetAlpha(1f);
    }

    void Start() => StartCoroutine(Fade(1f, 0f, fadeInDuration));

    public IEnumerator FadeOut()
    {
        yield return Fade(0f, 1f, fadeOutDuration);
    }

    IEnumerator Fade(float from, float to, float dur)
    {
        for (float t = 0; t < dur; t += Time.deltaTime)
        {
            SetAlpha(Mathf.Lerp(from, to, t / dur));
            yield return null;
        }
        SetAlpha(to);
    }

    void SetAlpha(float a)
    {
        rend.GetPropertyBlock(mpb);

        Color c = fadeColor; c.a = a;
        mpb.SetColor(id, c);
        mpb.SetColor("_Color", c);

        rend.SetPropertyBlock(mpb);
    }
}