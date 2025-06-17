using UnityEngine;
using TMPro;

/// <summary>
/// Watches one TimedAudio-component.  
/// Whenever the selected clip (by index) reaches the chosen moment
/// (start + delay or clip end) the script assigns the matching
/// string to a single TextMesh Pro component.
/// </summary>

public class ChangeTMPTextOnAudioEvents : MonoBehaviour
{
    #region Inspector
    [Header("Source of the audio")]
    [SerializeField] private MonoBehaviour timedAudioComponent;

    [Header("Which clips -- which texts")]
    [Tooltip("One index per clip in the TimedAudio component.")]
    [SerializeField] private int[] audioIndices;
    [Tooltip("Text that will be set when the matching clip fires.")]
    [TextArea(2, 4)]
    [SerializeField] private string[] newTexts;

    [Header("Target text")]
    [Tooltip("Leave empty -- looks for TMP_Text on the same GameObject.")]
    [SerializeField] private TMP_Text targetText;

    [Header("When to change? (global)")]
    [Tooltip("If TRUE: wait until the clip finishes.\n"
           + "If FALSE: use delayAfterStart for all clips.")]
    [SerializeField] private bool changeAfterClip = false;
    [Tooltip("Seconds after clip START before text change "
           + "(ignored if changeAfterClip = true).")]
    [SerializeField] private float delayAfterStart = 0f;
    #endregion

    #region Per-clip tracking
    private class ClipTracker
    {
        public AudioSource source;
        public bool started;
        public float startTime;
        public bool applied;
        public string text;
    }

    private ClipTracker[] trackers;
    #endregion

    #region Unity
    private void Start()
    {
        if (audioIndices.Length != newTexts.Length)
        {
            Debug.LogError($"{name}: audioIndices and newTexts must have same length.", this);
            enabled = false;
            return;
        }

        if (targetText == null)
            targetText = GetComponent<TMP_Text>();

        if (targetText == null)
        {
            Debug.LogError($"{name}: No TMP_Text found or assigned.", this);
            enabled = false;
            return;
        }

        AudioSource[] allSources = ResolveAudioSources();
        if (allSources == null)
        {
            Debug.LogError($"{name}: Could not read audioSources from TimedAudio component.", this);
            enabled = false;
            return;
        }

        trackers = new ClipTracker[audioIndices.Length];
        for (int i = 0; i < trackers.Length; i++)
        {
            int idx = audioIndices[i];
            if (idx < 0 || idx >= allSources.Length)
            {
                Debug.LogWarning($"{name}: audioIndex {idx} out of range.", this);
                continue;
            }

            trackers[i] = new ClipTracker
            {
                source = allSources[idx],
                text = newTexts[i]
            };
        }
    }

    private void Update()
    {
        if (trackers == null) return;

        foreach (var t in trackers)
        {
            if (t == null || t.applied || t.source == null) continue;


            if (!t.started && t.source.isPlaying)
            {
                t.started = true;
                t.startTime = Time.time;

                if (!changeAfterClip && delayAfterStart <= 0f)
                {
                    ApplyText(t);
                    continue;
                }
            }

            if (!t.started) continue;

            if (changeAfterClip)
            {
                if (!t.source.isPlaying)
                    ApplyText(t);
            }
            else
            {
                if (Time.time - t.startTime >= delayAfterStart)
                    ApplyText(t);
            }
        }
    }
    #endregion

    #region Helpers
    private void ApplyText(ClipTracker t)
    {
        targetText.text = t.text;
        t.applied = true;
    }

    private AudioSource[] ResolveAudioSources()
    {
        if (timedAudioComponent == null) return null;

        return timedAudioComponent switch
        {
            TimedAudioOnStart t => t.audioSources,
            TimedAudioOnGrab t => t.audioSources,
            TimedAudioOnTrigger t => t.audioSources,
            TimedAudioOnInsert t => t.audioSources,
            _ => null
        };
    }
    #endregion
}
