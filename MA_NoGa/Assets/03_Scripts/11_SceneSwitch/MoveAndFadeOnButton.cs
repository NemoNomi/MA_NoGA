using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

/// <summary>
/// • Drei × A (rechter Controller) in <windowSeconds> s → nächste Szene.
/// • Drei × X (linker Controller)  in <windowSeconds> s → aktuelle Szene neu laden.
/// Das Objekt bleibt szenenübergreifend erhalten (Singleton + DontDestroyOnLoad).
/// </summary>
public class SceneShortcutByTriplePress : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] private float windowSeconds = 5f;

    #region Singleton & Persistenz
    private static SceneShortcutByTriplePress instance;
    private void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(gameObject);          // Duplikat vermeiden
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);    // Überlebt Szenenwechsel
    }
    #endregion

    #region Zustände
    // Rechter Controller (nächste Szene)
    private int   nextPressCount;
    private float nextCountdown;
    private bool  prevRightPressed;

    // Linker Controller (Reload)
    private int   reloadPressCount;
    private float reloadCountdown;
    private bool  prevLeftPressed;
    #endregion

    private void Update()
    {
        // ---------- Rechter Controller: A-Button ----------
        var right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        if (right.isValid &&
            right.TryGetFeatureValue(CommonUsages.primaryButton, out bool rightPressed))
        {
            if (rightPressed && !prevRightPressed) RegisterNextPress();
            prevRightPressed = rightPressed;
        }

        // ---------- Linker Controller: X-Button ----------
        var left = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        if (left.isValid &&
            left.TryGetFeatureValue(CommonUsages.primaryButton, out bool leftPressed))
        {
            if (leftPressed && !prevLeftPressed) RegisterReloadPress();
            prevLeftPressed = leftPressed;
        }

        // Countdown-Timer tickt herunter
        if (nextCountdown   > 0f && (nextCountdown   -= Time.deltaTime) <= 0f) nextPressCount   = 0;
        if (reloadCountdown > 0f && (reloadCountdown -= Time.deltaTime) <= 0f) reloadPressCount = 0;
    }

    // ---------- Registrierung der A-Klicks ----------
    private void RegisterNextPress()
    {
        if (nextPressCount == 0) nextCountdown = windowSeconds;
        if (++nextPressCount >= 3)
        {
            LoadNextScene();
            nextPressCount = 0; nextCountdown = 0f;   // zurücksetzen
        }
    }

    // ---------- Registrierung der X-Klicks ----------
    private void RegisterReloadPress()
    {
        if (reloadPressCount == 0) reloadCountdown = windowSeconds;
        if (++reloadPressCount >= 3)
        {
            ReloadCurrentScene();
            reloadPressCount = 0; reloadCountdown = 0f;
        }
    }

    // ---------- Szene wechseln / neu laden ----------
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
