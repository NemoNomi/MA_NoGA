using UnityEngine;

public class BallMaterialTransfer : MonoBehaviour
{
    private Material myMaterial;

    void Start()
    {
        // Holt das eigene Material.
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
            myMaterial = renderer.material;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Prüfe, ob das berührte Objekt den Tag "Paintable" hat.
        if (collision.gameObject.CompareTag("Paintable"))
        {
            // Holt den Renderer des anderen Objekts.
            var otherRenderer = collision.gameObject.GetComponent<Renderer>();
            if (otherRenderer != null && myMaterial != null)
            {
                // Setzt das Material.
                otherRenderer.material = myMaterial;
            }
        }
    }
}
