using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// On the first press of the primary button on the chosen XR controller,
/// disables the “lazy follow” script, moves this object toward
/// the target while fading its Canvas Group, then loads the
/// next scene via FadeOutMainMenu if present.
/// </summary>

[RequireComponent(typeof(CanvasGroup))]
public class MoveAndFadeOnButton : MonoBehaviour
{
    #region Inspector
    [Header("XR Input")]
    [SerializeField] private XRNode controllerNode = XRNode.RightHand;

    [Header("Animation")]
    [SerializeField] private Transform target;
    [SerializeField] private float moveDuration = 1.5f;
    [SerializeField] private float fadeDuration = 1.5f;
    [SerializeField] private AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Optional Follow Script")]
    [SerializeField] private MonoBehaviour lazyFollowToDisable;
    #endregion

    #region Cached References
    private CanvasGroup cg;
    #endregion

    #region State
    private Vector3 startPos;
    private bool running;
    private bool prevPressed;
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        startPos = transform.position;

        if (!lazyFollowToDisable)
            lazyFollowToDisable = GetComponent("LazyFollow") as MonoBehaviour;
    }

    private void Update()
    {
        if (running || !target) return;

        var device = InputDevices.GetDeviceAtXRNode(controllerNode);
        if (!device.isValid) return;

        if (device.TryGetFeatureValue(CommonUsages.primaryButton, out bool pressed))
        {
            if (pressed && !prevPressed) StartSequence();
            prevPressed = pressed;
        }
    }
    #endregion

    #region Sequence
    private void StartSequence()
    {
        lazyFollowToDisable?.SetEnabled(false);
        StartCoroutine(AnimateAndLoad());
    }

    private IEnumerator AnimateAndLoad()
    {
        running = true;

        var fader = FindFirstObjectByType<FadeOutMainMenu>();
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;

        if (fader) fader.FadeAndLoad(nextScene);
        else SceneManager.LoadScene(nextScene);

        float t = 0f;
        float total = Mathf.Max(moveDuration, fadeDuration);

        while (t < total)
        {
            t += Time.deltaTime;

            float moveFrac = Mathf.Clamp01(t / moveDuration);
            transform.position = Vector3.Lerp(startPos, target.position, ease.Evaluate(moveFrac));

            cg.alpha = 1f - Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }

        cg.alpha = 0f;
    }
    #endregion
}

static class MonoBehaviourExt
{
    public static void SetEnabled(this MonoBehaviour mb, bool value) { if (mb) mb.enabled = value; }
}
