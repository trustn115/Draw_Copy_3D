using System;
using System.Collections;
using System.Collections.Generic;
using _Draw_Copy._Scripts.ControllerRelated;
using DG.Tweening;
using UnityEngine;

namespace _Draw_Copy._Scripts.GameplayRelated
{
    public class SprayColoring : MonoBehaviour
    {
        [HideInInspector] public bool canSpray;
        private LineRenderer _currentLine;
        private Vector3 _lastPos;
        public GameObject brush;

        public int targetNumOfPointsToReach;
        private Collider _collider;

        private void Start()
        {
            canSpray = true;
            _collider = GetComponent<Collider>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && canSpray)
            {
                CreateBrush();
                _collider.enabled = true;
            }

            if (Input.GetMouseButton(0) && canSpray)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 10))
                {
                    Vector3 hitPos = hit.point;
                    transform.position = new Vector3(hitPos.x, transform.position.y, hitPos.z);
                    if (hitPos != _lastPos && Vector3.Distance(hitPos, _lastPos) > 0.05f)
                    {
                        AddPoint(new Vector3(hitPos.x, transform.localPosition.y, hitPos.z));
                        _lastPos = hitPos;
                    }
                }
            }

            if (Input.GetMouseButtonUp(0) && canSpray)
            {
                _collider.enabled = false;
            }
        }

        private int _pointsReachCounter = 0;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 6)
            {
                print("mid point triggered - " + _pointsReachCounter);
                other.GetComponent<Collider>().enabled = false;
                _pointsReachCounter++;
                if (_pointsReachCounter > (int)(targetNumOfPointsToReach / 1.3f))
                {
                    ColoringController.instance.MinimumColoringReached();
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
            GameObject brushInst = Instantiate(brush);
            _currentLine = brushInst.GetComponent<LineRenderer>();
            brushInst.transform.parent = ColoringController.instance.stencil.transform;
            
            DOVirtual.DelayedCall(0.05f, () =>
            {
                _currentLine.SetPosition(0, new Vector3(transform.position.x, -1, transform.position.z));
                _currentLine.SetPosition(1, new Vector3(transform.position.x, -1, transform.position.z));
            });
        }
    }
}