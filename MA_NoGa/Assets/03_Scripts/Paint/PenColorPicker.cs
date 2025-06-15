using UnityEngine;

/// <summary>
/// Copies the color of any collider tagged colorSource
/// to the pen’s ineMaterial when they enter this trigger.
/// </summary>

public class PenColorPicker : MonoBehaviour
{
    #region Inspector
    [Tooltip("Material, that changes color.")]
    public Material lineMaterial;

    [Tooltip("Materials where the color is taken from.")]
    public string colorSourceTag = "ColorSource";
    #endregion

    #region Trigger
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(colorSourceTag)) return;

        Renderer objRenderer = other.GetComponent<Renderer>();
        if (objRenderer != null && objRenderer.material != null)
        {
            lineMaterial.color = objRenderer.material.color;
        }
    }
    #endregion
}