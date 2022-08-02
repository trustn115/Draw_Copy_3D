using System;
using System.Collections;
using System.Collections.Generic;
using _Draw_Copy._Scripts.ControllerRelated;
using DG.Tweening;
using UnityEngine;

namespace _Draw_Copy._Scripts.GameplayRelated
{
    public class RobotPen : MonoBehaviour
    {
        public static RobotPen instance;
        
        public GameObject brush;
        private LineRenderer _currentLine;
        public List<Transform> points;
        public Transform ink;
        public float inkFinishSpeed;
        [SerializeField] private bool _canDraw;

        private List<Vector3> _drawnPointList = new List<Vector3>();
        private Vector3 _defaultLinePos;

        public List<int> takes;
        public List<Shape> shapes;

        private void Awake()
        {
            instance = this;
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
                //StartCoroutine(FormTheShape());
                if(_loopCounter < takes.Count)
                    StartCoroutine(FormMultipleShapes());
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
                _currentLine.positionCount++;
                Vector3 newPoint = new Vector3(transform.position.x, -1, transform.position.z);
                _currentLine.SetPosition(_currentLine.positionCount - 1, newPoint);
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

        private int _loopCounter = 0, _pointsCounter = 0;
        IEnumerator FormMultipleShapes()
        {
            PlayerDrawing.instance.currentTakes = takes[_loopCounter];
            int loopNum = takes[_loopCounter++];
            for (int i = 0; i < loopNum; i++)
            {
                CompareDrawings.instance.drawnPointsMovePos = shapes[_pointsCounter].transform.position;
                List<Transform> pointsTaken = shapes[_pointsCounter++].points;
                CreateBrush(pointsTaken);
                for (int j = 0; j < pointsTaken.Count; j++)
                {
                    Vector3 newMovePos = new Vector3(pointsTaken[j].position.x, transform.position.y, pointsTaken[j].position.z);
                    transform.DOMove(newMovePos, 0.025f).SetEase(Ease.Linear);
                    _drawnPointList.Add(newMovePos);
                    yield return  new WaitForSeconds(0.025f);
                }
                CompareDrawings.instance.targetPts = _drawnPointList;
                _drawnPointList = new List<Vector3>();
                SoundsController.instance.roboDrawSource.enabled = false;
            }

            /*if (_loopCounter < takes.Count)
            {
                StartCoroutine(FormMultipleShapes());
                _loopCounter++;
            }*/
            DOVirtual.DelayedCall(0.4f,
                () => { MainController.instance.SetActionType(GameState.PlayerDrawing); });
            
            yield return null;
        }
        void CreateBrush(List<Transform> pointsTaken)
        {
            _canDraw = false;
            GameObject brushInst = Instantiate(brush);
            _currentLine = brushInst.GetComponent<LineRenderer>();
            Vector3 penMovePos = new Vector3(pointsTaken[0].position.x, transform.position.y, pointsTaken[0].position.z);

            transform.position = penMovePos;
            _currentLine.SetPosition(0, new Vector3(transform.position.x, -1, transform.position.z));
            _currentLine.SetPosition(1, new Vector3(transform.position.x, -1, transform.position.z));
            _canDraw = true;

            SoundsController.instance.roboDrawSource.enabled = true;
            /*transform.DOMove(penMovePos, 0.5f).OnComplete(() =>
            {
                currentLine.SetPosition(0, new Vector3(transform.position.x, -1, transform.position.z));
                currentLine.SetPosition(1, new Vector3(transform.position.x, -1, transform.position.z));
                _canDraw = true;
            });*/
        }
    }
    
}
