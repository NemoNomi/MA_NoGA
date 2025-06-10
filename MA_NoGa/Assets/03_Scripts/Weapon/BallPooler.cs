using System.Collections.Generic;
using UnityEngine;

public class BallPooler : MonoBehaviour
{
    [Tooltip("Prefab des Projektils.")]
    public GameObject projectilePrefab;

    [Tooltip("Anzahl der vorerstellten Projektile.")]
    public int poolSize = 20;

    private List<GameObject> pool;

    void Awake()
    {
        pool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(projectilePrefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }
        return null;
    }
}
