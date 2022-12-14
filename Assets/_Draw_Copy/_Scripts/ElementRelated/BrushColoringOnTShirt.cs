using System.Collections;
using System.Collections.Generic;
using _Draw_Copy._Scripts.ControllerRelated;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Draw_Copy._Scripts.ElementRelated
{ 
    public class BrushColoringOnTShirt : MonoBehaviour 
    {
    public static BrushColoringOnTShirt instance;
    
    private LineRenderer _currentLine;
        private Vector3 _lastPos;
        public Transform raypoint;
        private bool _isCanvasLevel;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            _isCanvasLevel = ColoringController.instance.isCanvasLevel;
        }

        private bool _timeCountingStarted;
        private float _timeCounter;
        
        private void Update()
        {
            if (_timeCountingStarted)
            {
                _timeCounter += Time.deltaTime;
            }
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.currentSelectedGameObject)
            {
                CreateBrush();
                if (!_timeCountingStarted)
                {
                    _timeCountingStarted = true;
                }
                //SoundsController.instance.playerDrawSource.enabled = true;
            }

            if (Input.GetMouseButton(0) && !EventSystem.current.currentSelectedGameObject)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100))
                {
                    Vector3 hitPos = hit.point;
                    transform.position = new Vector3(hitPos.x, hitPos.y, raypoint.position.z);;
                    if (hitPos != _lastPos && Vector3.Distance(hitPos, _lastPos) > 0.001f)
                    {
                        if(!_isCanvasLevel) AddPoint(new Vector3(raypoint.position.x, -1, raypoint.position.z));
                        else AddPoint(new Vector3(raypoint.position.x, raypoint.position.y, -0.9f));
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
                if (_timeCounter >= 3.5f)
                {
                    _timeCountingStarted = false;
                    UIController.instance.coloringDoneButton.SetActive(true);
                }
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
            _currentLine = ColoringController.instance.SetupBrush();
            _currentLine.gameObject.SetActive(false);
            //ColoringController.instance.outlinesList.Add(_currentLine);
            DOVirtual.DelayedCall(0.05f, () =>
            {
                _currentLine.SetPosition(0, new Vector3(raypoint.position.x, raypoint.position.y,-0.857f));
                    _currentLine.SetPosition(1, new Vector3(raypoint.position.x, raypoint.position.y,-0.857f));
                    _currentLine.gameObject.SetActive(true);
            });
        }
    }
}
