using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillWithLineRenderer : MonoBehaviour
{
    public List<Transform> points;
    LineRenderer lineRenderer;

    private List<Transform> _midPointsList = new List<Transform>();

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        Vector3 midpoint = GetShapeMidPoint(points);
        
        /*Vector3 linePos0 = (points[0].position - midpoint) / 2;
        lineRenderer.startWidth = (points[0].position - midpoint).magnitude;
        lineRenderer.endWidth = (points[0].position - midpoint).magnitude;
        lineRenderer.SetPosition(0, linePos0);
        
        Vector3 linePos1 = (points[1].position - midpoint) / 2;
        lineRenderer.startWidth = (points[1].position - midpoint).magnitude;
        lineRenderer.endWidth = (points[1].position - midpoint).magnitude;
        lineRenderer.SetPosition(1, linePos0);*/
        
        GameObject midPointCenter = new GameObject("Midpoint Center");
        for (int i = 0; i < points.Count; i++)
        {
            GameObject mid = new GameObject("Midpoint(" + i + ")");
            Vector3 midPointPos = (points[i].position - midpoint) / 2f;
            mid.transform.position = midPointPos;
            _midPointsList.Add(mid.transform);
            mid.transform.parent = midPointCenter.transform;
        }
        midPointCenter.transform.position = midpoint;

        for (int i = 0; i < _midPointsList.Count; i++)
        {
            lineRenderer.startWidth = (points[i].position - midpoint).magnitude + 0.05f;
            lineRenderer.endWidth = (points[i].position - midpoint).magnitude + 0.05f;
            if (i > 1) lineRenderer.positionCount++;
            lineRenderer.SetPosition(i, _midPointsList[i].position);
        }
    }
    
    Vector3 GetShapeMidPoint(List<Transform> shapePoints)
    {
        float xTotal, zTotal;
        xTotal = zTotal = 0;
        for (int i = 0; i < shapePoints.Count; i++)
        {
            xTotal += shapePoints[i].position.x;
            zTotal += shapePoints[i].position.z;
        }
        return new Vector3(xTotal / shapePoints.Count, -1, zTotal / shapePoints.Count);
    }
}
