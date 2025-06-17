using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] private int targetBuildIndex = 1;
    #endregion

    #region Fields
    private TeleportationArea _tpArea;
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        _tpArea = GetComponent<TeleportationArea>();
        _tpArea.teleporting.AddListener(OnTeleporting);
    }

    private void OnDestroy() =>
        _tpArea.teleporting.RemoveListener(OnTeleporting);
    #endregion

    #region Event Handlers
    private void OnTeleporting(TeleportingEventArgs _) =>
        SceneManager.LoadScene(targetBuildIndex);
    #endregion
}
