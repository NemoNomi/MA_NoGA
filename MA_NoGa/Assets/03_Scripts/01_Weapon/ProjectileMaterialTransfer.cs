using UnityEngine;

/// 
/// Copies this objects material onto any collider tagged “Paintable”
/// when the two objects collide.
///
public class ProjectileMaterialTransfer : MonoBehaviour
{
    #region Cached Data
    private Material ownMaterial;
    #endregion

    private void Start()
    {
        var r = GetComponent<Renderer>();
        if (r) ownMaterial = r.material;
    }

    #region Collision
    private void OnCollisionEnter(Collision col)
    {
        if (!ownMaterial || !col.gameObject.CompareTag("Paintable")) return;

        var targetRenderer = col.gameObject.GetComponent<Renderer>();
        if (targetRenderer) targetRenderer.material = ownMaterial;
    }
    #endregion
}
