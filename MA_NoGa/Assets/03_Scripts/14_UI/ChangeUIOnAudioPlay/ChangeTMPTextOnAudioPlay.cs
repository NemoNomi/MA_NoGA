using UnityEngine;
using TMPro;

/// <summary>
/// Changes a TMP_Text when a specific clip starts playing on a shared AudioSource,
/// based on its index in a known AudioClip array.
/// </summary>
public class ChangeTMPTextOnAudioClipIndex : MonoBehaviour
{
    [Header("Audio Tracking")]
    [Tooltip("AudioSource that plays the clips.")]
    [SerializeField] private AudioSource audioSource;

    [Tooltip("Reference list of clips (e.g., same as in trigger script).")]
    [SerializeField] private AudioClip[] referenceClips;

    [Header("Clip Index → Text Mapping")]
    [Tooltip("Texts to assign when corresponding clip (by index) starts.")]
    [TextArea(2, 4)]
    [SerializeField] private string[] texts;

    [Header("Target Text")]
    [Tooltip("Leave empty to auto-detect on this GameObject.")]
    [SerializeField] private TMP_Text targetText;

    private int lastClipIndex = -1;

    private void Start()
    {
        if (!audioSource)
        {
            Debug.LogError($"{name}: No AudioSource assigned.");
            enabled = false;
            return;
        }

        if (referenceClips == null || referenceClips.Length == 0)
        {
            Debug.LogError($"{name}: No referenceClips assigned.");
            enabled = false;
            return;
        }

        if (texts.Length != referenceClips.Length)
        {
            Debug.LogWarning($"{name}: Texts array length does not match referenceClips.");
        }

        if (!targetText)
            targetText = GetComponent<TMP_Text>();

        if (!targetText)
        {
            Debug.LogError($"{name}: No TMP_Text found or assigned.");
            enabled = false;
            return;
        }
    }

    private void Update()
    {
        if (!audioSource.isPlaying || audioSource.clip == null)
            return;

        int index = System.Array.IndexOf(referenceClips, audioSource.clip);

        if (index >= 0 && index != lastClipIndex)
        {
            if (index < texts.Length)
            {
                targetText.text = texts[index];
                lastClipIndex = index;
            }
            else
            {
                Debug.LogWarning($"{name}: No text defined for clip index {index}.");
            }
        }
    }
}
