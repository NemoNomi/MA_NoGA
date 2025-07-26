using System.Collections.Generic;
using UnityEngine;

///
/// Maintains a fixed size pool of projectile prefabs and returns the first
/// inactive instance on request.
/// Takes the defined Material for the Projectiles.
///

public class ProjectilePooler : MonoBehaviour
{
    #region Inspector
    [Header("Pool Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int poolSize = 20;
    [SerializeField] private Renderer materialSource;

    #endregion

    private readonly List<GameObject> pool = new();

    private void Awake()
    {

        Material weaponMat = null;

        if (materialSource != null)
        {
            weaponMat = materialSource.sharedMaterial;
        }
        else if (transform.parent != null)
        {
            weaponMat = transform.parent.GetComponentInChildren<Renderer>(true)?.sharedMaterial;
        }

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(projectilePrefab, transform);

            if (weaponMat != null)
            {
                foreach (Renderer rend in obj.GetComponentsInChildren<Renderer>(true))
                    rend.sharedMaterial = weaponMat;
            }

            obj.SetActive(false);
            pool.Add(obj);
        }
    }


    public GameObject GetPooledObject()
    {
        foreach (var obj in pool)
            if (!obj.activeInHierarchy) return obj;

        return null;
    }
}
