using System;
using System.Collections;
using System.Collections.Generic;
using _Draw_Copy._Scripts.ControllerRelated;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;


namespace _Draw_Copy._Scripts.ElementRelated
{
    public class BrushColoringElement : MonoBehaviour
    {
        public static BrushColoringElement instance;
        
        private LineRenderer _currentLine;
        private Vector3 _lastPos;
        public GameObject brush;
        public Transform raypoint;

        private void Awake()
        {
            instance = this;
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.currentSelectedGameObject)
            {
                CreateBrush();
                //SoundsController.instance.playerDrawSource.enabled = true;
            }

            if (Input.GetMouseButton(0) && !EventSystem.current.currentSelectedGameObject)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100))
                {
                    Vector3 hitPos = hit.point;
                    transform.position = new Vector3(hitPos.x, transform.position.y, hitPos.z);
                    if (hitPos != _lastPos && Vector3.Distance(hitPos, _lastPos) > 0.001f)
                    {
                        AddPoint(new Vector3(raypoint.position.x, -1, raypoint.position.z));
                        _lastPos = hitPos;
                    }
                }
            }
            else
            {
                _currentLine = null;
            }

            if (Input.GetMouseButtonUp(0) && !EventSystem.current.currentSelectedGameObject)
            {
                //ColoringController.instance.AddNewShapes(GetTransformsOutOfPoints(_drawnPointList));
                //SoundsController.instance.playerDrawSource.enabled = false;
            }
        }
        
        void AddPoint(Vector3 pointPos)
        {
            _currentLine.positionCount++;
            int positionIndex = _currentLine.positionCount - 1;
            _currentLine.SetPosition(positionIndex, pointPos);
        }

        void CreateBrush()
        {
            GameObject brushInst = Instantiate(brush);
            brushInst.SetActive(false);
            _currentLine = brushInst.GetComponent<LineRenderer>();
            _currentLine.startColor = ColoringController.instance.currentBrushColor;
            _currentLine.endColor = ColoringController.instance.currentBrushColor;
            
            _currentLine.startWidth = ColoringController.instance.currentBrushWidth;
            _currentLine.endWidth = ColoringController.instance.currentBrushWidth;
            //ColoringController.instance.outlinesList.Add(_currentLine);
            DOVirtual.DelayedCall(0.05f, () =>
            {
                _currentLine.SetPosition(0, new Vector3(raypoint.position.x, -1, raypoint.position.z));
                _currentLine.SetPosition(1, new Vector3(raypoint.position.x, -1, raypoint.position.z));
                brushInst.SetActive(true);
            });
        }
    }   
}
