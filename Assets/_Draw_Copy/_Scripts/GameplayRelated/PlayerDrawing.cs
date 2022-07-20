using System;
using System.Collections;
using System.Collections.Generic;
using _Draw_Copy._Scripts.ControllerRelated;
using DG.Tweening;
using UnityEngine;

namespace _Draw_Copy._Scripts.GameplayRelated
{
    public class PlayerDrawing : MonoBehaviour
    {
        private LineRenderer _currentLine;
        private Vector3 _lastPos;
        public GameObject brush;
        private Camera _camera;
        public Transform pen;
        Transform _raypoint;

        private bool _canDraw;

        private void Start()
        {
            _camera = Camera.main;
            _raypoint = pen.GetChild(0);
        }
        private void OnEnable()
        {
            MainController.GameStateChanged += GameManager_GameStateChanged;
        }
        private void OnDisable()
        {
            MainController.GameStateChanged -= GameManager_GameStateChanged;
        }
        void GameManager_GameStateChanged(GameState newState, GameState oldState)
        {
            if(newState==GameState.PlayerDrawing)
            {
                _canDraw = true;
            }
            if(newState==GameState.RoboDrawing)
            {
                _canDraw = false;
            }
            
        }

        private void Update()
        {
            {
            }
            if (Input.GetMouseButtonDown(0) && _canDraw)
            {
                CreateBrush();
            }

            if (Input.GetMouseButton(0) && _canDraw)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100))
                {
                    Vector3 hitPos = hit.point;
                    pen.position = new Vector3(hitPos.x, pen.position.y, hitPos.z);
                    if (hitPos != _lastPos)
                    {
                        AddPoint(new Vector3(hitPos.x, -1, hitPos.z));
                        _lastPos = hitPos;
                    }
                }
            }
            else
            {
                _currentLine = null;
            }
        }

        void CreateBrush()
        {
            GameObject brushInst = Instantiate(brush);
            _currentLine = brushInst.GetComponent<LineRenderer>();
            DOVirtual.DelayedCall(0.05f, () =>
            {
                _currentLine.SetPosition(0, new Vector3(pen.position.x, -1, pen.position.z));
                _currentLine.SetPosition(1, new Vector3(pen.position.x, -1, pen.position.z));
            });
        }

        void AddPoint(Vector3 pointPos)
        {
            _currentLine.positionCount++;
            int positionIndex = _currentLine.positionCount - 1;
            _currentLine.SetPosition(positionIndex, pointPos);
        }
    }
}