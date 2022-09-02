using System;
using System.Collections;
using System.Collections.Generic;
using _Draw_Copy._Scripts.ControllerRelated;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Draw_Copy._Scripts.GameplayRelated
{
    public class PlayerDrawingGirlBack : MonoBehaviour
    {
        public static PlayerDrawingGirlBack instance;

        private LineRenderer _currentLine;
        private Vector3 _lastPos;
        public GameObject brush;
        private Camera _camera;
        public Transform pen;
        public Transform ink;
        public float inkFinishSpeed;
        Transform _raypoint;

        private bool _canDraw;
        public List<Vector3> _drawnPointList = new List<Vector3>();
        public int currentTakes;
        [SerializeField] private int _takesCounter = 0;
        public bool isCanvasLevel;

        private Vector3 penOrigPos;
        public GameObject drawFx;
        public float zPos = -0.91f;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            _camera = Camera.main;
            _raypoint = pen.GetChild(0);
            penOrigPos = new Vector3(1.5f, transform.position.y, -1.75f);
            /*drawFx = Instantiate(drawFx, Vector3.zero, Quaternion.identity);
            drawFx.SetActive(false);*/
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
            if (newState == GameState.PlayerDrawing)
            {
                _canDraw = true;
            }

            if (newState == GameState.RoboDrawing)
            {
                _canDraw = false;
            }
        }

        private bool _mouseDownRecorded;
        private bool _startVibrating, _canShowFx;
        private float _timeGap, fxTimeGap;

        private void Update()
        {
            if (_startVibrating)
            {
                _timeGap += Time.deltaTime;
                if (_timeGap >= 0.3)
                {
                    Vibration.Vibrate(10);
                    _timeGap = 0;
                }
            }

            if (_canShowFx)
            {
                fxTimeGap += Time.deltaTime;
                if (fxTimeGap >= 0.65)
                {
                    fxTimeGap = 0;
                    ShowDrawFx();
                }
            }

            if (Input.GetMouseButtonDown(0) && _canDraw && !_mouseDownRecorded &&
                !EventSystem.current.currentSelectedGameObject)
            {
                CreateBrush();
                _mouseDownRecorded = true;
                SoundsController.instance.playerDrawSource.enabled = true;
                _startVibrating = true;
                _canShowFx = true;
                ShowDrawFx();
                //drawFx.SetActive(true);
            }

            if (Input.GetMouseButton(0) && _canDraw && _mouseDownRecorded &&
                !EventSystem.current.currentSelectedGameObject)
            {
                Vector3 hitPos = GetHitPoint();
                if (!isCanvasLevel) pen.position = new Vector3(hitPos.x, pen.position.y, hitPos.z);
                else pen.position = new Vector3(hitPos.x, hitPos.y, pen.position.z);
                if (hitPos != _lastPos /* && Vector3.Distance(hitPos, _lastPos) > 0.001f*/)
                {
                    if (!isCanvasLevel) AddPoint(new Vector3(hitPos.x, -1, hitPos.z));
                    else AddPoint(new Vector3(hitPos.x, hitPos.y, zPos));
                    _lastPos = hitPos;
                    _drawnPointList.Add(hitPos);
                    BezierInterpolate();
                    //drawFx.transform.position = new Vector3(hitPos.x, -1, hitPos.z);
                }


                if (ink.localScale.y > 0)
                {
                    ink.localScale = new Vector3(ink.localScale.x, ink.localScale.y - Time.deltaTime * inkFinishSpeed,
                        ink.localScale.z);
                }
            }
            else
            {
                _currentLine = null;
            }

            if (Input.GetMouseButtonUp(0) && _canDraw && _mouseDownRecorded &&
                !EventSystem.current.currentSelectedGameObject)
            {
                //ColoringController.instance.AddNewShapes(GetTransformsOutOfPoints(_drawnPointList));
                _takesCounter++;
                CompareDrawings.instance.drawnPts = _drawnPointList;
                CompareDrawings.instance.StartCoroutine(CompareDrawings.instance.CompareShape());
                if (_takesCounter == currentTakes)
                {
                    /*DOVirtual.DelayedCall(0.5f,
                        () => { MainController.instance.SetActionType(GameState.RoboDrawing); });*/
                    _takesCounter = 0;
                    _shapesCounter++;
                    GameController.instance.ChangeToRoboDrawingState();
                }
                //CheckIfAllShapesDrawn();
                _drawnPointList = new List<Vector3>();
                _mouseDownRecorded = false;
                SoundsController.instance.playerDrawSource.enabled = false;
                _startVibrating = false;
                _canShowFx = false;
                //drawFx.SetActive(false);
            }
        }

        void ShowDrawFx()
        {
            Vector3 hitPoint = GetHitPoint();
            Vector3 spawnPos = Vector3.zero;
            if (!isCanvasLevel) spawnPos =  new Vector3(hitPoint.x, -1, hitPoint.z);
            else spawnPos = new Vector3(hitPoint.x, hitPoint.y, zPos);
            Instantiate(drawFx, spawnPos, Quaternion.identity);
        }
        
        private static List<Vector3> bezierDrawingPoints;
        public void BezierInterpolate()
        {
            BezierPath bezierPath = new BezierPath ();
            bezierPath.Interpolate(_drawnPointList, 0.3f);
            bezierDrawingPoints = bezierPath.GetDrawingPoints2();
            _currentLine.SetVertexCount(bezierDrawingPoints.Count);
            _currentLine.SetPositions (bezierDrawingPoints.ToArray ());
        }

        Vector3 GetHitPoint()
        {
            Vector3 hitPoint = Vector3.zero;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100))
            {
                print(hit.collider.name);
                hitPoint = hit.point;
                print(hitPoint);
            }

            return new Vector3(hitPoint.x, hitPoint.y, zPos);;
        }

        private int _shapesCounter = 0;

        public void CheckIfAllShapesDrawn(int playerTurnCount)
        {
            if (playerTurnCount >= RobotPen.instance.takes.Count)
            {
                CompareDrawings.instance.StartCoroutine(CompareDrawings.instance.CheckLevelState());
            }
            else
            {
                DOVirtual.DelayedCall(0.5f,
                    () => { MainController.instance.SetActionType(GameState.RoboDrawing); });
            }

            //_shapesCounter++;
            /*if (_shapesCounter >= RobotPen.instance.shapes.Count)
            {
                CompareDrawings.instance.StartCoroutine(CompareDrawings.instance.CheckLevelState());
            }
            else
            {
                DOVirtual.DelayedCall(0.5f,
                    () => { MainController.instance.SetActionType(GameState.RoboDrawing); });
            }*/
        }

        void CreateBrush()
        {
            GameObject brushInst = Instantiate(brush);
            brushInst.SetActive(false);
            _currentLine = brushInst.GetComponent<LineRenderer>();
            _currentLine.sortingLayerName = "outline";
            //ColoringController.instance.outlinesList.Add(_currentLine);
            DOVirtual.DelayedCall(0.05f, () =>
            {
                if (!isCanvasLevel)
                {
                    _currentLine.SetPosition(0, new Vector3(pen.position.x, -1, pen.position.z));
                    _currentLine.SetPosition(1, new Vector3(pen.position.x, -1, pen.position.z));
                }
                else
                {
                    _currentLine.positionCount = 2;
                    _currentLine.SetPosition(0, new Vector3(pen.position.x, pen.position.y, zPos));
                    _currentLine.SetPosition(1, new Vector3(pen.position.x, pen.position.y, zPos));
                }

                brushInst.SetActive(true);
            });
        }

        void AddPoint(Vector3 pointPos)
        {
            _currentLine.positionCount++;
            int positionIndex = _currentLine.positionCount - 1;
            _currentLine.SetPosition(positionIndex, pointPos);
        }

        List<Transform> GetTransformsOutOfPoints(List<Vector3> points)
        {
            List<Transform> drawnPointsAsTransforms = new List<Transform>();
            for (int i = 0; i < points.Count; i++)
            {
                GameObject pointObj = new GameObject("User Drawn Point(" + i + ")");
                pointObj.transform.position = points[i];
                drawnPointsAsTransforms.Add(pointObj.transform);
            }

            return drawnPointsAsTransforms;
        }
    }
}