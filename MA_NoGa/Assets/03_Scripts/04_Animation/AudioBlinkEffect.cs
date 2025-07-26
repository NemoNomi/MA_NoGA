using UnityEngine;

///
/// Switches between Colors when AudioSource plays Clip.
///

public class AudioBlinkEffect : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource;

    [Header("Target Material")]
    public Renderer targetRenderer;
    public Color baseColor = Color.white;
    public Color blinkColor = Color.red;
    public float blinkSpeed = 5f;

    [Header("Emission Settings")]
    public float emissionIntensity = 7f;

    private Material _material;
    private static readonly string EmissionColorProperty = "_EmissionColor";


    void Start()
    {
        if (targetRenderer != null)
        {
            _material = targetRenderer.material;
            _material.EnableKeyword("_EMISSION");
        }
        else
        {
            Debug.LogError("Target Renderer is not assigned.");
        }
    }

    void Update()
    {
        if (audioSource == null || _material == null)
            return;

        if (audioSource.isPlaying)
        {
            float t = Mathf.PingPong(Time.time * blinkSpeed, 1f);
            Color currentColor = Color.Lerp(baseColor, blinkColor, t);
            _material.color = currentColor;
            _material.SetColor(EmissionColorProperty, currentColor * emissionIntensity);
        }
        else
        {
            if (_material.color != baseColor)
            {
                _material.color = baseColor;
                _material.SetColor(EmissionColorProperty, baseColor * emissionIntensity);
            }
        }
    }
}