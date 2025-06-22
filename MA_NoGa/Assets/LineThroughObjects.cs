using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Verbindet die angegebenen Transforms in der Reihenfolge der Liste
/// mit einem LineRenderer.  Fügt optional den eigenen Transform als
/// ersten Punkt hinzu (praktisch, wenn die Linie an diesem Objekt beginnt).
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class LineThroughObjects : MonoBehaviour
{
    [Tooltip("Reihenfolge der Punkte, die die Linie passieren soll.")]
    public List<Transform> points = new List<Transform>();

    [Tooltip("Soll der Transform dieses GameObjects als Startpunkt dienen?")]
    public bool includeSelfAsStart = false;

    LineRenderer lr;

void Awake()
{
    lr = GetComponent<LineRenderer>();

    // minimale, aber wichtige Basiswerte
    lr.useWorldSpace   = true;
    lr.widthMultiplier = 0.005f;           // 5 mm
    lr.material.enableInstancing = true;   // falls Shader es zulässt
    lr.alignment       = LineAlignment.View;   // zeigt immer zur Kamera
}


    void LateUpdate()      // LateUpdate = nachdem alle Objekte bewegt wurden
    {
        if (lr == null || points.Count == 0) return;

        int countOffset = includeSelfAsStart ? 1 : 0;
        lr.positionCount = points.Count + countOffset;

        // 0) eigener Transform (falls gewünscht)
        if (includeSelfAsStart)
            lr.SetPosition(0, transform.position);

        // 1…N) alle weiteren Punkte aus der Liste
        for (int i = 0; i < points.Count; i++)
            lr.SetPosition(i + countOffset, points[i].position);
    }
}
