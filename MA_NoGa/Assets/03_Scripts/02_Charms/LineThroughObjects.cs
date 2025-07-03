using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Connects transform with LineRenderer
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class LineThroughObjects : MonoBehaviour
{
    public List<Transform> points = new List<Transform>();

    public bool includeSelfAsStart = false;

    LineRenderer lr;

void Awake()
{
    lr = GetComponent<LineRenderer>();

    lr.useWorldSpace   = true;
    lr.widthMultiplier = 0.005f;
    lr.material.enableInstancing = true;
    lr.alignment       = LineAlignment.View;
}


    void LateUpdate()
    {
        if (lr == null || points.Count == 0) return;

        int countOffset = includeSelfAsStart ? 1 : 0;
        lr.positionCount = points.Count + countOffset;

        if (includeSelfAsStart)
            lr.SetPosition(0, transform.position);

        for (int i = 0; i < points.Count; i++)
            lr.SetPosition(i + countOffset, points[i].position);
    }
}
