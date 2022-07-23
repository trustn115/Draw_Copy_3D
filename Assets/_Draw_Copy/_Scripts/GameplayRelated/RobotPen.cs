using System;
using System.Collections;
using System.Collections.Generic;
using _Draw_Copy._Scripts.ControllerRelated;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Draw_Copy._Scripts.GameplayRelated
{
    public class RobotPen : MonoBehaviour
    {
        private LineRenderer _line;
        public List<Transform> points;
        public Transform ink;
        public float inkFinishSpeed;
        private bool _canDraw;

        private List<Vector3> _drawnPointList = new List<Vector3>();
        private Vector3 _defaultLinePos;
        
        private void Start()
        {
            _line = GetComponent<LineRenderer>();
            _defaultLinePos =  new Vector3(transform.position.x, -1, transform.position.z);
            _line.SetPosition(0, points[0].position);
            _line.SetPosition(1, _defaultLinePos);
            
            _drawnPointList.Add(points[0].position);
            _drawnPointList.Add(_defaultLinePos);
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
            if(newState==GameState.RoboDrawing)
            {
                _canDraw = true;
                StartCoroutine(FormTheShape());
            }

            if (newState == GameState.PlayerDrawing)
            {
                _canDraw = false;
            }
            
        }
        private void Update()
        {
            if(_canDraw)
            {
                _line.positionCount++;
                Vector3 newPoint = new Vector3(transform.position.x, -1, transform.position.z);
                _line.SetPosition(_line.positionCount - 1, newPoint);
            }
        }

        IEnumerator FormTheShape()
        {
            for (int i = 0; i < points.Count; i++)
            {
                Vector3 newMovePos = new Vector3(points[i].position.x, transform.position.y, points[i].position.z);
                transform.DOMove(newMovePos, 0.12f).SetEase(Ease.Linear);
                _drawnPointList.Add(newMovePos);
                
                if(ink.localScale.y > 0)
                {
                    ink.localScale = new Vector3(ink.localScale.x, ink.localScale.y - Time.deltaTime * inkFinishSpeed,
                        ink.localScale.z);
                }
                yield return new WaitForSeconds(0.08f);
            }
            CompareDrawings.instance.targetPts = _drawnPointList;
            print("Robo Drawn Points = " + _drawnPointList.Count);
            MainController.instance.SetActionType(GameState.PlayerDrawing);
        }
    }   
}
