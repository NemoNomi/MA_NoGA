using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

/// <summary>
/// Loads the scene defined by targetBuildIndex
/// whenever the player successfully teleports
/// onto the attached TeleportationArea.
/// </summary>

[RequireComponent(typeof(TeleportationArea))]
public sealed class SceneLoadOnTeleport : MonoBehaviour
{
    #region Inspector
    [SerializeField] private int        targetBuildIndex = 1;
    [SerializeField] private string     animationBool    = "Open";
    [SerializeField] private Animator[] animators;
    #endregion

    #region Fields
    private TeleportationArea _area;
    #endregion

    #region Unity
    private void Awake()
    {
        _area = GetComponent<TeleportationArea>();

        if (animators == null || animators.Length == 0)
            animators = GetComponentsInChildren<Animator>(true);

        _area.selectEntered.AddListener(OnSelectEntered);
        _area.selectExited .AddListener(OnSelectExited);
        _area.teleporting  .AddListener(OnTeleporting);
    }

    private void OnDestroy()
    {
        _area.selectEntered.RemoveListener(OnSelectEntered);
        _area.selectExited .RemoveListener(OnSelectExited);
        _area.teleporting .RemoveListener(OnTeleporting);
    }
    #endregion

    #region Event-Handler
    private void OnSelectEntered(SelectEnterEventArgs _) => SetDoor(true);
    private void OnSelectExited (SelectExitEventArgs  _) => SetDoor(false);
    private void OnTeleporting  (TeleportingEventArgs _) => SceneManager.LoadScene(targetBuildIndex);
    #endregion

    #region Helpers
    private void SetDoor(bool open)
    {
        foreach (var a in animators)
            a.SetBool(animationBool, open);
    }
    #endregion
}
