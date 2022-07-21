using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class CompareDrawings : MonoBehaviour
{
    public static CompareDrawings instance;

    public List<Vector3> targetPts, drawnPts;
    private int correctCounter = 0, wrongCounter = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //StartCoroutine(CompareShape(targetPts, drawnPts));
    }

    public IEnumerator CompareShape()
    {
        List<Transform> targetPoints, drawnPoints;
        targetPoints = new List<Transform>();
        drawnPoints = new List<Transform>();
        for (int i = 0; i < targetPts.Count; i++)
        {
            GameObject pt = new GameObject("PointTarget " + i);
            pt.transform.position = targetPts[i];
            targetPoints.Add(pt.transform);
        }
        for (int i = 0; i < drawnPts.Count; i++)
        {
            GameObject pt = new GameObject("PointDrawn " + i);
            pt.transform.position = drawnPts[i];
            drawnPoints.Add(pt.transform);
        }
        
        Vector3 targetShapeMidPoint = GetShapeMidPoint(targetPoints);
        Vector3 drawnShapeMidPoint = GetShapeMidPoint(drawnPoints);

        float targetShapeScale = Vector3.Distance(targetShapeMidPoint, targetPoints[0].position);
        float drawnShapeScale = Vector3.Distance(drawnShapeMidPoint, drawnPoints[0].position);
        print("target shape scale = " + targetShapeScale);
        print("drawn shape scale = " + drawnShapeScale);

        float scaleAmount = 0;
        if (drawnShapeScale > targetShapeScale)
        {
            if (drawnShapeScale < 1)
                scaleAmount = 1 - (drawnShapeScale - targetShapeScale);
            else scaleAmount = Mathf.Abs(1 - (drawnShapeScale - targetShapeScale));
        }
        else scaleAmount = 1 + (targetShapeScale - drawnShapeScale);

        //put all the drawn points in a parent
        GameObject drawnPointsParent = new GameObject("DrawnPointsParent");
        drawnPointsParent.transform.position = drawnShapeMidPoint;
        for (int i = 0; i < drawnPoints.Count; i++)
        {
            drawnPoints[i].parent = drawnPointsParent.transform;
            //drawnPoints[i].transform.position = new Vector3(drawnPoints[i].position.x, 0, drawnPoints[i].position.z);
        }
        //scale down the parent to (ParentScale - scaleAmount)
        drawnPointsParent.transform.localScale = Vector3.one * scaleAmount;
        print("Scale amount = " + scaleAmount);
        //parent position = targetShapeMidPoint
        drawnPointsParent.transform.position = targetShapeMidPoint;
        //calculate each drawnShapePoint's( (drawnPoints/targetPoints)th point ) distance from each targetPoint
        int skipper = 0;
        if(drawnPoints.Count >= targetPoints.Count)
            skipper = Mathf.CeilToInt(drawnPoints.Count/targetPoints.Count);
        print("Skipper = " + skipper);

        // un parent the drawn points before calculating distance
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < drawnPoints.Count; i++)
        {
            drawnPoints[i].parent = null;
            drawnPoints[i].position = new Vector3(drawnPoints[i].position.x, -1, drawnPoints[i].position.z);
        }
        print("Drawn point[0] pos = " + drawnPoints[0].position);
        
        List<float> highDist = new List<float>();
        List<float> lowDist = new List<float>();
        for (int i = 0; i < targetPoints.Count; i++)
        {
            //print(i);
            float d = Vector3.Distance(targetPoints[i].position, drawnPoints[i + skipper].position);
            print("dst = " + d);
            if(d > 0.75f) highDist.Add(d);
            else lowDist.Add(d);
        }

        if (highDist.Count > lowDist.Count || highDist.Count == lowDist.Count) wrongCounter++;
        else correctCounter++;
        if(correctCounter > wrongCounter) print("WIN !!!");
        else print("LOSE !!!");

        /*float totalDist = 0;
        for (int i = 0; i < lowDist.Count; i++)
        {
            totalDist += lowDist[i];
        }

        float avgDist = totalDist / lowDist.Count;
        print("Avg dist = " + avgDist);
        //calculate % err  or
        float percMatch = 1 - avgDist;
        if(percMatch > 0.5f)
            print("WIN !!!");
        else print("LOSE !!!");*/
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