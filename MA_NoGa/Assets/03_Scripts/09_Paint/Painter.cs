using UnityEngine;
using UnityEngine.InputSystem;


public class Painter : MonoBehaviour
{
    [Header("Pen Settings")]
    public Transform tip;
    public Material lineMaterial;
    [Range(0.001f, 0.1f)] public float penWidth = 0.01f;
    [Range(0.001f, 0.1f)] public float minDistance = 0.03f;
    [Range(0, 10)] public int capVertices = 4;
    [Range(0, 10)] public int cornerVertices = 5;

    [Header("Grab Interactable")]
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    [Header("Input Action References")]
    public InputActionReference leftTriggerAction;
    public InputActionReference rightTriggerAction;

    private LineRenderer currentLine;
    private int index = 0;
    private bool isTriggerHeld = false;

    void OnEnable()
    {
        if (leftTriggerAction != null)
        {
            leftTriggerAction.action.Enable();
            leftTriggerAction.action.performed += _ => isTriggerHeld = true;
            leftTriggerAction.action.canceled += _ => isTriggerHeld = false;
        }

        if (rightTriggerAction != null)
        {
            rightTriggerAction.action.Enable();
            rightTriggerAction.action.performed += _ => isTriggerHeld = true;
            rightTriggerAction.action.canceled += _ => isTriggerHeld = false;
        }
    }

    void OnDisable()
    {
        if (leftTriggerAction != null)
        {
            leftTriggerAction.action.performed -= _ => isTriggerHeld = true;
            leftTriggerAction.action.canceled -= _ => isTriggerHeld = false;
            leftTriggerAction.action.Disable();
        }

        if (rightTriggerAction != null)
        {
            rightTriggerAction.action.performed -= _ => isTriggerHeld = true;
            rightTriggerAction.action.canceled -= _ => isTriggerHeld = false;
            rightTriggerAction.action.Disable();
        }
    }

    void Update()
    {
        if (isTriggerHeld && grabInteractable != null && grabInteractable.isSelected)
        {
            Draw();
        }
        else
        {
            currentLine = null;
        }
    }

    void Draw()
    {
        if (currentLine == null)
        {
            GameObject lineObj = new GameObject("Line");
            currentLine = lineObj.AddComponent<LineRenderer>();

            Material newMat = new Material(lineMaterial);
            currentLine.material = newMat;

            currentLine.material.color = lineMaterial.color;

            currentLine.startWidth = penWidth;
            currentLine.endWidth = penWidth;
            currentLine.positionCount = 1;
            currentLine.SetPosition(0, tip.position);

            currentLine.numCornerVertices = cornerVertices;
            currentLine.numCapVertices = capVertices;

            index = 1;
        }
        else
        {
            Vector3 lastPos = currentLine.GetPosition(index - 1);

            if (Vector3.Distance(lastPos, tip.position) > minDistance)
            {
                index++;
                currentLine.positionCount = index;
                currentLine.SetPosition(index - 1, tip.position);
            }
        }
    }

}