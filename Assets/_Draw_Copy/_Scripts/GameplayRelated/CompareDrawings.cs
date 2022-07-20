using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CompareDrawings : MonoBehaviour
{
    public static CompareDrawings instance;

    public List<Transform> targetPts, drawnPts;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CompareShape(targetPts, drawnPts);
    }

    public void CompareShape(List<Transform> targetPoints, List<Transform> drawnPoints)
    {
        Vector3 targetShapeMidPoint = GetShapeMidPoint(targetPoints);
        Vector3 drawnShapeMidPoint = GetShapeMidPoint(drawnPoints);

        float targetShapeScale = Vector3.Distance(targetShapeMidPoint, targetPoints[0].position);
        float drawnShapeScale = Vector3.Distance(drawnShapeMidPoint, drawnPoints[0].position);

        float scaleAmount = 0;
        if (drawnShapeScale > targetShapeScale)
        {
            scaleAmount = drawnShapeScale - targetShapeScale;
        }
        //put all the drawn points in a parent
        GameObject drawnPointsParent = new GameObject("DrawnPointsParent");
        drawnPointsParent.transform.position = targetShapeMidPoint;
        //scale down the parent to (ParentScale - scaleAmount)
        //parent position = targetShapeMidPoint
        //calculate each drawnShapePoint's( (drawnPoints/targetPoints)th point ) distance from each targetPoint
        //calculate % error
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