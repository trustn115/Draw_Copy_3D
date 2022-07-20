using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ShapeScaleDown : MonoBehaviour
{
    public List<Transform> points;
    public Transform center;

    public float scaleFactor;

    private void Start()
    {
        float xTotal, zTotal;
        xTotal = zTotal = 0;
        for (int i = 0; i < points.Count; i++)
        {
            xTotal += points[i].position.x;
            zTotal += points[i].position.z;
        }
        Vector3 midPoint = new Vector3(xTotal / points.Count, -1, zTotal / points.Count);
        center.position = midPoint;

        ScaleDownShape();
    }

    void ScaleDownShape()
    {
        for (int i = 0; i < points.Count; i++)
        {
            Vector3 dir = center.position - points[i].position;
            points[i].DOMove(points[i].position - (dir / 2), 0.5f);
        }
    }
}
