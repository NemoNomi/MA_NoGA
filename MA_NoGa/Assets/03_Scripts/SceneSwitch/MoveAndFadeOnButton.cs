using UnityEngine;
using UnityEngine.XR;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CanvasGroup))]
public class MoveAndFadeOnButton : MonoBehaviour
{
    [Header("XR-Input")]
    public XRNode controllerNode = XRNode.RightHand;

    [Header("Animation")]
    public Transform target;
    public float moveDuration = 1.5f;
    public float fadeDuration = 1.5f;
    public AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Deactivated Script on A Click")]
    public MonoBehaviour lazyFollowToDisable;

    CanvasGroup cg;
    Vector3 startPos;
    bool running, prevPressed;

    void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        startPos = transform.position;

        if (lazyFollowToDisable == null)
            lazyFollowToDisable = GetComponent("LazyFollow") as MonoBehaviour;
    }

    void Update()
    {
        if (running || target == null) return;

        var device = InputDevices.GetDeviceAtXRNode(controllerNode);
        if (!device.isValid) return;

        if (device.TryGetFeatureValue(CommonUsages.primaryButton, out bool pressed))
        {
            if (pressed && !prevPressed)
            {
                if (lazyFollowToDisable) lazyFollowToDisable.enabled = false;
                StartCoroutine(AnimateAndLoad());
            }
            prevPressed = pressed;
        }
    }

    IEnumerator AnimateAndLoad()
    {
        running = true;

        FadeOutMainMenu fader = FindObjectOfType<FadeOutMainMenu>();
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (fader) fader.FadeAndLoad(nextIndex);
        else SceneManager.LoadScene(nextIndex);

        float t = 0f;
        float total = Mathf.Max(moveDuration, fadeDuration);

        while (t < total)
        {
            t += Time.deltaTime;

            float moveFrac = Mathf.Clamp01(t / moveDuration);
            transform.position =
                Vector3.Lerp(startPos, target.position, ease.Evaluate(moveFrac));

            cg.alpha = 1f - Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }

        cg.alpha = 0f;
    }

}