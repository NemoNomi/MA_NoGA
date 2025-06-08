using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class FadeIn : MonoBehaviour
{
    public float fadeDuration = 1.0f;

    Renderer rend;
    MaterialPropertyBlock mpb;
    static readonly int baseColorID = Shader.PropertyToID("_BaseColor");

    void Awake()
    {
        rend = GetComponent<Renderer>();
        mpb = new MaterialPropertyBlock();

        SetAlpha(1f);
    }

    void Start()
    {
        StartCoroutine(FadeInAndDestroy());
    }

    IEnumerator FadeInAndDestroy()
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(fadeDuration, .01f);
            SetAlpha(Mathf.Lerp(1f, 0f, t));
            yield return null;
        }
        Destroy(gameObject);
    }

    void SetAlpha(float a)
    {
        rend.GetPropertyBlock(mpb);

        Color c = mpb.HasVector(baseColorID)
                  ? mpb.GetColor(baseColorID)
                  : Color.black;
        c.a = a;

        mpb.SetColor(baseColorID, c);
        mpb.SetColor("_Color", c);
        rend.SetPropertyBlock(mpb);
    }
}
