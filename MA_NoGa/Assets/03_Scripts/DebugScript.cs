using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

/// <summary>
/// Click A Button 3 times on right Controller within 5 seconds to skip the sene. Click X Button 3 times on left Controller within 5 seconds to restart the scene.
/// Singleton
/// </summary>
public class DebugScript : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] private float windowSeconds = 5f;

    #region Singleton & Persistenz
    private static DebugScript instance;
    private void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    #region Zustände
    private int   nextPressCount;
    private float nextCountdown;
    private bool  prevRightPressed;

    private int   reloadPressCount;
    private float reloadCountdown;
    private bool  prevLeftPressed;
    #endregion

    private void Update()
    {
        var right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        if (right.isValid &&
            right.TryGetFeatureValue(CommonUsages.primaryButton, out bool rightPressed))
        {
            if (rightPressed && !prevRightPressed) RegisterNextPress();
            prevRightPressed = rightPressed;
        }

        var left = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        if (left.isValid &&
            left.TryGetFeatureValue(CommonUsages.primaryButton, out bool leftPressed))
        {
            if (leftPressed && !prevLeftPressed) RegisterReloadPress();
            prevLeftPressed = leftPressed;
        }

        if (nextCountdown   > 0f && (nextCountdown   -= Time.deltaTime) <= 0f) nextPressCount   = 0;
        if (reloadCountdown > 0f && (reloadCountdown -= Time.deltaTime) <= 0f) reloadPressCount = 0;
    }

    private void RegisterNextPress()
    {
        if (nextPressCount == 0) nextCountdown = windowSeconds;
        if (++nextPressCount >= 3)
        {
            LoadNextScene();
            nextPressCount = 0; nextCountdown = 0f;
        }
    }

    private void RegisterReloadPress()
    {
        if (reloadPressCount == 0) reloadCountdown = windowSeconds;
        if (++reloadPressCount >= 3)
        {
            ReloadCurrentScene();
            reloadPressCount = 0; reloadCountdown = 0f;
        }
    }

    private void LoadNextScene()
    {
        int next = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(next);
    }

    private void ReloadCurrentScene()
    {
        int cur = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(cur);
    }
}
