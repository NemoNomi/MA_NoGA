using UnityEngine;

public class BallMaterialTransfer : MonoBehaviour
{
    private Material myMaterial;

    void Start()
    {
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
            myMaterial = renderer.material;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Paintable"))
        {
            var otherRenderer = collision.gameObject.GetComponent<Renderer>();
            if (otherRenderer != null && myMaterial != null)
            {
                otherRenderer.material = myMaterial;
            }
        }
    }
}
