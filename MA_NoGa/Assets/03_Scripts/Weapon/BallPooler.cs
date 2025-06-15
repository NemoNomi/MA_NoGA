using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Maintains a fixed-size pool of ball prefabs and returns the first
/// inactive instance on request.
/// </summary>
public class BallPooler : MonoBehaviour
{
    #region Inspector
    [Header("Pool Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int poolSize = 20;
    #endregion

    #region State
    private readonly List<GameObject> pool = new();
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            var obj = Instantiate(projectilePrefab, transform);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }
    #endregion

    #region Public API
    public GameObject GetPooledObject()
    {
        foreach (var obj in pool)
            if (!obj.activeInHierarchy)
                return obj;

        return null;
    }
    #endregion
}
