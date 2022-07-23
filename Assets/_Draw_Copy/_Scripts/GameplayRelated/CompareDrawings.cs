using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using _Draw_Copy._Scripts.ControllerRelated;
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
    
    public List<Transform> targetPoints = new List<Transform>(), drawnPoints = new List<Transform>();
    private List<Transform> lowDist;
    public IEnumerator CompareShape()
    {
        for (int i = 0; i < targetPts.Count; i++)
        {
            GameObject pt = new GameObject("PointTarget " + i);
            pt.transform.position = new Vector3(targetPts[i].x, -1, targetPts[i].z);
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
        
        /*Vector3 fromAngle = drawnPoints[0].position - drawnPointsParent.transform.position;
        Vector3 toAngle = targetPoints[0].position - drawnPointsParent.transform.position;
        float angle = Vector3.Angle(fromAngle, toAngle);
        drawnPointsParent.transform.eulerAngles = Vector3.up * angle;
        print("ANGLE= " + angle);*/
        // un parent the drawn points before calculating distance
        for (int i = 0; i < drawnPoints.Count; i++)
        {
            drawnPoints[i].parent = null;
            drawnPoints[i].position = new Vector3(drawnPoints[i].position.x, -1, drawnPoints[i].position.z);
        }
        print("Drawn point[0] pos = " + drawnPoints[0].position);
        
        List<float> highDist = new List<float>();
        lowDist = new List<Transform>();
        
        for (int i = 0; i < targetPoints.Count; i++)
        {
            for (int j = 0; j < drawnPoints.Count; j++)
            {
                float d = Vector3.Distance(targetPoints[i].position, drawnPoints[j].position);
                if(d < 0.1f)
                {
                    if (!lowDist.Contains(drawnPoints[j]))
                    {
                        lowDist.Add(drawnPoints[j]);
                        print("d = " + d);    
                    }
                }
                //else highDist.Add(d);
            }
        }
        
        print("Low Dist Count 1 = " + lowDist.Count);
        if(lowDist.Count > 0)
        {
            if (lowDist.Count > drawnPoints.Count)
            {
                StartCoroutine(LevelCondition(true));
            }
            else if (lowDist.Count < drawnPoints.Count)
            {
                if (lowDist.Count > (drawnPoints.Count * 50) / 100)
                {
                    StartCoroutine(LevelCondition(true));
                }
                else
                {
                    StartCoroutine(LevelCondition(false));
                }
            }
            else if (lowDist.Count == drawnPoints.Count)
            {
                StartCoroutine(LevelCondition(true));
            }
        }
        else
        {
            StartCoroutine(LevelCondition(false));
        }
        print("Low Dist Count 2 = " + lowDist.Count);
        yield return null;
    }

    IEnumerator LevelCondition(bool pass)
    {
        if(pass)
        {
            UIController.instance.winConfetti.SetActive(true);
            yield return new WaitForSeconds(0.75f);
            int perc = Mathf.CeilToInt((lowDist.Count / drawnPoints.Count) * 100);
            UIController.instance.StartCoroutine(
                UIController.instance.ShowMatchPercentage(perc));
            yield return new WaitForSeconds(2f);
            MainController.instance.SetActionType(GameState.Levelwin);
        }
        else
        {
            yield return new WaitForSeconds(2f);
            MainController.instance.SetActionType(GameState.Levelfail);
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