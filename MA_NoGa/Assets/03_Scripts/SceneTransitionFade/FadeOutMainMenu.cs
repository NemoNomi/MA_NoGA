using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(Renderer))]
public class FadeOutMainMenu : MonoBehaviour
{
    [Header("References")]
    [Tooltip("MeshRenderer des Quads vor der Kamera")]
    public Renderer quadRenderer;

    [Header("Fade-Out-Settings")]
    [Range(0.1f, 5f)]
    public float fadeDuration = 1.0f;
    public Color fadeColor = Color.black;

    Material quadMat;

    void Awake()
    {
        if (quadRenderer == null) quadRenderer = GetComponent<Renderer>();

        quadMat = quadRenderer.material;
        SetAlpha(0f);
    }

    public void FadeAndLoad(int buildIndex)
    {
        StartCoroutine(FadeOutThenActivate(buildIndex));
    }

    IEnumerator FadeOutThenActivate(int buildIndex)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(fadeDuration, .01f);
            SetAlpha(Mathf.Lerp(0f, 1f, t));
            yield return null;
        }
        SetAlpha(1f);
        AsyncOperation op = SceneManager.LoadSceneAsync(buildIndex);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f) yield return null;

        op.allowSceneActivation = true;
    }

    void SetAlpha(float a)
    {
        Color c = fadeColor; c.a = a;
        if (quadMat.HasProperty("_BaseColor"))
            quadMat.SetColor("_BaseColor", c);
        else
            quadMat.color = c;
    }
}
