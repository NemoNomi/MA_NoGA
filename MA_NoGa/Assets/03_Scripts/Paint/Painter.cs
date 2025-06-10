using UnityEngine;
using UnityEngine.InputSystem;

public class Painter : MonoBehaviour

{
    [Header("Pen Settings")]
    [Tooltip("Die Spitze des Stifts (leeres GameObject an der Spitze).")]
    public Transform tip;

    [Tooltip("Material für die Linie.")]
    public Material lineMaterial;

    [Tooltip("Breite der Linie.")]
    [Range(0.001f, 0.1f)]
    public float penWidth = 0.01f;

    [Tooltip("Farbe der Linie.")]
    public Color penColor = Color.red;

    [Tooltip("Mindestabstand, um einen neuen Punkt zu setzen.")]
    [Range(0.001f, 0.1f)]
    public float minDistance = 0.03f;

    [Header("Linien-Glättung")]
    [Tooltip("Anzahl der abgerundeten Enden (0 = eckig, höher = runder).")]
    [Range(0, 10)]
    public int capVertices = 4;

    [Tooltip("Anzahl der abgerundeten Ecken (0 = eckig, höher = runder).")]
    [Range(0, 10)]
    public int cornerVertices = 5;

    [Header("Input Action References")]
    public InputActionReference leftTriggerAction;
    public InputActionReference rightTriggerAction;

    private LineRenderer currentLine;
    private int index = 0;
    private bool isDrawing = false;

    void OnEnable()
    {
        if (leftTriggerAction != null)
        {
            leftTriggerAction.action.Enable();
            leftTriggerAction.action.performed += _ => isDrawing = true;
            leftTriggerAction.action.canceled += _ => isDrawing = false;
        }

        if (rightTriggerAction != null)
        {
            rightTriggerAction.action.Enable();
            rightTriggerAction.action.performed += _ => isDrawing = true;
            rightTriggerAction.action.canceled += _ => isDrawing = false;
        }
    }

    void OnDisable()
    {
        if (leftTriggerAction != null)
        {
            leftTriggerAction.action.performed -= _ => isDrawing = true;
            leftTriggerAction.action.canceled -= _ => isDrawing = false;
            leftTriggerAction.action.Disable();
        }

        if (rightTriggerAction != null)
        {
            rightTriggerAction.action.performed -= _ => isDrawing = true;
            rightTriggerAction.action.canceled -= _ => isDrawing = false;
            rightTriggerAction.action.Disable();
        }
    }

    void Update()
    {
        if (isDrawing)
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
            currentLine.material = lineMaterial;
            currentLine.startColor = penColor;
            currentLine.endColor = penColor;
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