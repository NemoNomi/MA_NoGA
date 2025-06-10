using UnityEngine;

public class PenColorPicker : MonoBehaviour
{
    [Tooltip("Das Material des Stifts, dessen Farbe angepasst wird.")]
    public Material lineMaterial;

    [Tooltip("Nur Materialien von Objekten mit diesem Tag werden übernommen.")]
    public string colorSourceTag = "ColorSource";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(colorSourceTag)) return;

        Renderer objRenderer = other.GetComponent<Renderer>();
        if (objRenderer != null && objRenderer.material != null)
        {
            lineMaterial.color = objRenderer.material.color;
        }
    }
}
