using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

/// <summary>
/// Moves the object back to a specified socket position when it is not held and not placed in a socket.
/// </summary>

public class AutoReturnToSocket : MonoBehaviour
{
    #region Inspector

    public Transform targetSocket;
    public float returnSpeed = 2f;

    #endregion

    #region Private Fields

    private XRGrabInteractable grabInteractable;
    private bool isHeld = false;

    #endregion

    #region Unity Events

    private void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    private void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrabbed);
        grabInteractable.selectExited.RemoveListener(OnReleased);
    }

    private void Update()
    {
        if (!isHeld && !IsInSocket())
        {
            transform.position = Vector3.MoveTowards(transform.position, targetSocket.position, returnSpeed * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetSocket.rotation, returnSpeed * 100f * Time.deltaTime);
        }
    }

    #endregion

    #region Interaction Events

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        isHeld = true;
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        isHeld = false;
    }

    #endregion

    #region Helpers

    private bool IsInSocket()
    {
        return grabInteractable.isSelected &&
               grabInteractable.interactorsSelecting.Count > 0 &&
               grabInteractable.interactorsSelecting[0] is XRSocketInteractor;
    }

    #endregion
}
