using System;
using System.Collections;
using System.Collections.Generic;
using _Draw_Copy._Scripts.ControllerRelated;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class ColorShapes : MonoBehaviour
{
    public static ColorShapes instance;
    public GameObject shapeColor;
    public GameObject stencil;
    
    List<Transform> drawnPoints = new List<Transform>();
    private Vector3 _stencilOrigPos;

    private void Awake()
    {
        instance = this;
    }

    public List<Transform> _midPointsList = new List<Transform>();
    
    public void ColorShape(List<Transform> pts)
    {
        /*for (int i = 0; i < drawnPts.Count; i++)
        {
            GameObject pt = new GameObject("PointDrawn " + i);
            pt.transform.position = drawnPts[i];
            drawnPoints.Add(pt.transform);
        }*/

        drawnPoints = pts;

        Vector3 midpoint = GetShapeMidPoint(drawnPoints);
        GameObject midPointCenter = new GameObject("Midpoint Center");

        //bring the stencil
        StencilController.instance.StartCoroutine(StencilController.instance.MoveStencilForColoring(midpoint, drawnPoints));
        
        _midPointsList = new List<Transform>();
        for (int i = 0; i < drawnPoints.Count; i++)
        {
            GameObject mid = new GameObject("Midpoint(" + i + ")");
            Vector3 midPointPos = (drawnPoints[i].position - midpoint) / 2f;
            mid.transform.position = midPointPos;
            _midPointsList.Add(mid.transform);
            mid.transform.parent = midPointCenter.transform;
        }
        midPointCenter.transform.position = midpoint;
        AddCollidersToMidPoints();
        StartCoroutine(ShowColoredShape(midpoint));
    }

    [HideInInspector] public LineRenderer fill;
    
    //--todo FORMING THE FILLED COLOR
    private int _nextShapeSortingOrder = 5;
    private int _sortingOrder;
    private LineRenderer _lastShape;
    IEnumerator ShowColoredShape(Vector3 midpoint)
    {
        if (_lastShape != null) ColoringController.instance.ManageColoredShapeSorting(_lastShape);
        fill = Instantiate(shapeColor).GetComponent<LineRenderer>();
        ColoringController.instance.coloredShapeList.Add(fill);
        _lastShape = fill;
        fill.sortingOrder = _nextShapeSortingOrder++;
        
        for (int i = 0; i < _midPointsList.Count; i++)
        {
            fill.startWidth = (drawnPoints[i].position - midpoint).magnitude + 0.03f;
            fill.endWidth = (drawnPoints[i].position - midpoint).magnitude + 0.03f;
            //yield return new WaitForSeconds(0.01f);
            if (i > 1) fill.positionCount++;
            fill.SetPosition(i, _midPointsList[i].position);
        }
        yield return null;
    }

    public void AddCollidersToMidPoints()
    {
        for (int i = 0; i < _midPointsList.Count; i++)
        {
            BoxCollider boxCollider = _midPointsList[i].gameObject.AddComponent(typeof(BoxCollider)) as BoxCollider;
            boxCollider.size = Vector3.one * 0.1f;
            boxCollider.isTrigger = true;
            _midPointsList[i].gameObject.layer = 6;
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