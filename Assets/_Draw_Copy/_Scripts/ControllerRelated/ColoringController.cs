using System;
using System.Collections;
using System.Collections.Generic;
using _Draw_Copy._Scripts.GameplayRelated;
using DG.Tweening;
using UnityEngine;

namespace  _Draw_Copy._Scripts.ControllerRelated
{
    public class ColoringController : MonoBehaviour
    {
        public static ColoringController instance;
        
        [Header("STENCIL RELATED")]
        public GameObject stencil;
        public GameObject sprayBottle;
        public GameObject peelingHelp;
        public Transform peelingPartsParent;
        public Transform finalPeelingPart, finalFoldingPart;
        public float peelingSpeed;
        private Vector3 _stencilOrigPos;

        private bool _canPeel;
        private float _peelingTime = 0;

        public Color currentShapeColor;

        [Header("SHAPE RELATED")]
        public List<Shape> userDrawnShapes;

        private void Awake()
        {
            instance = this;
        }

        private Vector3 _peelingParentOrigScale;

        private void Start()
        {
            _peelingParentOrigScale = peelingPartsParent.localScale;
        }

        private int _shapesCounter;
        public void AddNewShapes(List<Transform> points)
        {
            Shape newShape = Instantiate(new GameObject("UserDrawnShape").AddComponent<Shape>());
            newShape.points = points;
            userDrawnShapes.Add(newShape);
            _shapesCounter++;
            if (_shapesCounter == RobotPen.instance.shapes.Count)
            {
                //begin stencil work
                StartCoroutine(BeginStencilWork());
            }
        }

        IEnumerator BeginStencilWork()
        {
            yield return new WaitForSeconds(1);
            CameraController.instance.ChangeCameraForColoring();
            yield return new WaitForSeconds(1);
            for (int i = 0; i < userDrawnShapes.Count; i++)
            {
                ColorShapes.instance.ColorShape(userDrawnShapes[i].points);   
            }
        }

        private bool _peelingActivated;
        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && _peelingActivated)
            {
                peelingHelp.SetActive(false);
                _canPeel = true;
                _peelingActivated = false;
            }
            if (Input.GetMouseButton(0) && _canPeel)
            {
                if (peelingPartsParent.localScale.x < 53)
                {
                    peelingPartsParent.localScale += Vector3.one * peelingSpeed * Time.deltaTime;
                }
                else
                {
                    _canPeel = false;
                    PeelCompletely();
                }
            }

            if (Input.GetMouseButtonUp(0) && _canPeel)
            {
                if (peelingPartsParent.localScale.x < 53)
                {
                    peelingPartsParent.DOScale(_peelingParentOrigScale, 1);
                }
            }
        }

        void PeelCompletely()
        {
            finalFoldingPart.gameObject.SetActive(false);
            finalFoldingPart.gameObject.SetActive(true);
            finalPeelingPart.gameObject.SetActive(true);
            
            finalPeelingPart.DOScaleY(2.05f, 1);
            finalFoldingPart.DOLocalMoveY(-6.35f,1).OnComplete(() =>
            {
                stencil.SetActive(false); 
            });
        }
        public IEnumerator MoveStencilForColoring(Vector3 stencilMovePos, List<Transform> drawnPoints)
        {
            _stencilOrigPos = stencil.transform.position;
            stencil.SetActive(true);
            stencil.transform.DOMove(stencilMovePos, 1);
            
            yield return new WaitForSeconds(2);
            //show the spray bottle & spray help
            sprayBottle.SetActive(true);
            sprayBottle.GetComponent<SprayColoring>().targetNumOfPointsToReach = drawnPoints.Count;
            sprayBottle.transform.DOMove(stencilMovePos, 1);
            //calculate how many points touched - if more than 60%
            
            //show the peeling help
            //peel on mousedown
            //disable stencil after peeling
            //READYYY
            //stencil.transform.DOMove(_stencilOrigPos, 1);
        }

        public void MinimumColoringReached()
        {
            sprayBottle.SetActive(false);
            peelingHelp.SetActive(true);
            _peelingActivated = true;
            //UIController.instance.peelHelpButton.SetActive(true);
        }

        public void OnPeelingHelpClicked()
        {
            peelingHelp.SetActive(false);
            _canPeel = true;
            LineRenderer filledColor = GetComponent<ColorShapes>().filledColor;
            filledColor.startColor = currentShapeColor;
            filledColor.endColor = currentShapeColor;
        }
    }
}
