using System.Collections;
using System.Collections.Generic;
using _Draw_Copy._Scripts.ControllerRelated;
using _Draw_Copy._Scripts.GameplayRelated;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerDrawingTShirtElement : MonoBehaviour
{
    public static PlayerDrawingTShirtElement instance;

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
        
        public Transform hand;
        private Vector3 _handOrigPos;
        public Transform brushParent;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            _camera = Camera.main;
            _raypoint = pen.GetChild(0);
            _handOrigPos = hand.localPosition;
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
        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && _canDraw)
            {
                CreateBrush();
                _mouseDownRecorded = true;
                SoundsController.instance.playerDrawSource.enabled = true;
                hand.DOLocalMove(_handOrigPos, 0.25f);
            }

            if (Input.GetMouseButton(0) && _canDraw)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100))
                {
                    print(hit.collider.name);
                    Vector3 hitPos = hit.point;
                    if(!isCanvasLevel) pen.position = new Vector3(hitPos.x, pen.position.y, hitPos.z);
                    else pen.position = new Vector3(hitPos.x, hitPos.y, pen.position.z);
                    
                        if(!isCanvasLevel) AddPoint(new Vector3(hitPos.x, -1, hitPos.z));
                        else AddPoint(new Vector3(hitPos.x, hitPos.y,hitPos.z));
                        _lastPos = hitPos;
                        _drawnPointList.Add(hitPos);
                    
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
            if (Input.GetMouseButtonUp(0) && _canDraw)
            {
                //ColoringController.instance.AddNewShapes(GetTransformsOutOfPoints(_drawnPointList));
                _takesCounter++;
                //CompareDrawings.instance.drawnPts = _drawnPointList;
                //CompareDrawings.instance.StartCoroutine(CompareDrawings.instance.CompareShape());
                

                _drawnPointList = new List<Vector3>();
                _mouseDownRecorded = false;
                SoundsController.instance.playerDrawSource.enabled = false;
                hand.DOLocalMoveX(-6.22f, 0.45f);
                hand.DOLocalMoveZ(-3f, 0.45f);
                
            }
        }

        private int _shapesCounter = 0;
        
        

        void CreateBrush()
        {
            GameObject brushInst = Instantiate(brush);
            brushInst.transform.parent = brushParent;
            brushInst.SetActive(false);
            _currentLine = brushInst.GetComponent<LineRenderer>();
            _currentLine.sortingLayerName = "outline";
            //_currentLine.useWorldSpace = false;
            //ColoringController.instance.outlinesList.Add(_currentLine);
            DOVirtual.DelayedCall(0.05f, () =>
            {
                if(!isCanvasLevel)
                {
                    _currentLine.SetPosition(0, new Vector3(pen.position.x, -1, pen.position.z));
                    _currentLine.SetPosition(1, new Vector3(pen.position.x, -1, pen.position.z));
                }
                else
                {
                    _currentLine.SetPosition(0, new Vector3(pen.position.x, pen.position.y,-0.857f));
                    _currentLine.SetPosition(1, new Vector3(pen.position.x, pen.position.y,-0.857f));
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
