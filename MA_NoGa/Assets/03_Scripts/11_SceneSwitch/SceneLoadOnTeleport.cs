using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

///
/// Loads a target scene when the player teleports onto this Teleportation Area.
/// Optionally redirects to a fallback scene once a given number of other
/// scenes have been visited during the current play session.
///

[RequireComponent(typeof(TeleportationArea))]
public sealed class SceneLoadOnTeleport : MonoBehaviour
{
    #region Inspector: Target
    [Header("Primary Target")]
    [SerializeField] private int targetBuildIndex = 1;

    [Tooltip(
        "If ≥ 0 and enough side scenes have already been visited, this "
      + "build-index is loaded instead of the primary target."
    )]
    [SerializeField] private int fallbackBuildIndex = -1;

    [Tooltip(
        "How many visited side scenes are required before the fallback "
      + "takes effect (for your flow: 2)."
    )]
    [Min(1)]
    [SerializeField] private int visitedThreshold = 2;

    [Header("Visited Tracking")]
    [Tooltip(
        "Should the loaded scene be counted as 'visited' in this session?\n"
      + "TRUE for doors that enter Scene 3/4.\n"
      + "FALSE for doors that return to any Academy scene version."
    )]
    [SerializeField] private bool markTargetAsVisited = false;
    #endregion

    #region Inspector: (optional) Door Animation
    [SerializeField] private string animationBool = "Open";
    [SerializeField] private Animator[] animators;
    #endregion

    #region Fields
    private TeleportationArea _area;
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        _area = GetComponent<TeleportationArea>();

        if (animators == null || animators.Length == 0)
            animators = GetComponentsInChildren<Animator>(true);

        _area.selectEntered.AddListener(OnSelectEntered);
        _area.selectExited.AddListener(OnSelectExited);
        _area.teleporting.AddListener(OnTeleporting);
    }

    private void OnDestroy()
    {
        _area.selectEntered.RemoveListener(OnSelectEntered);
        _area.selectExited.RemoveListener(OnSelectExited);
        _area.teleporting.RemoveListener(OnTeleporting);
    }
    #endregion

    #region Event Handlers
    private void OnSelectEntered(SelectEnterEventArgs _) => SetDoor(true);
    private void OnSelectExited(SelectExitEventArgs _) => SetDoor(false);

    private void OnTeleporting(TeleportingEventArgs _)
    {
        int nextIndex = targetBuildIndex;

        if (fallbackBuildIndex >= 0 &&
            SceneVisitTracker.VisitedCount >= visitedThreshold)
        {
            nextIndex = fallbackBuildIndex;
        }

        if (markTargetAsVisited)
            SceneVisitTracker.MarkVisited(nextIndex);

        SceneManager.LoadScene(nextIndex);
    }
    #endregion

    #region Helpers
    private void SetDoor(bool open)
    {
        foreach (var a in animators)
            a.SetBool(animationBool, open);
    }
    #endregion
}