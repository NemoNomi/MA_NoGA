using UnityEngine;

/// <summary>
/// Switches between Colors when AudioSource plays Clip.
/// </summary>

public class AudioBlinkEffect : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource;

    [Header("Target Material")]
    public Renderer targetRenderer;
    public Color baseColor = Color.white;
    public Color blinkColor = Color.red;
    public float blinkSpeed = 5f;

    private Material _material;
    private bool _isBlinking;

    void Start()
    {
        if (targetRenderer != null)
            _material = targetRenderer.material;
        else
            Debug.LogError("Target Renderer is not assigned.");
    }

    void Update()
    {
        if (audioSource == null || _material == null)
            return;

        if (audioSource.isPlaying)
        {
            float t = Mathf.PingPong(Time.time * blinkSpeed, 1f);
            _material.color = Color.Lerp(baseColor, blinkColor, t);
        }
        else
        {
            if (_material.color != baseColor)
                _material.color = baseColor;
        }
    }
}
