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
        private Vector3 _stencilOrigPos;

        private bool _canPeel;
        private float _peelingTime = 0;

        public List<Color>  shapeColors;
        public Color currentShapeColor;

        [Header("SHAPE RELATED")]
        public List<Shape> userDrawnShapes;

        public int takesIndex = 0;
        public List<int> takes;

        public List<LineRenderer> outlinesList = new List<LineRenderer>(), coloredShapeList = new List<LineRenderer>();

        private void Awake()
        {
            instance = this;
        }

        private int _sortingCounter = 0;
        public void ManageColoredShapeSorting(LineRenderer shape)
        {
            if (_sortingCounter == 0) shape.sortingOrder = 0;
            _sortingCounter++;
        }

        private Vector3 _peelingParentOrigScale;

        private void Start()
        {
            takes = RobotPen.instance.takes;
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
                MainController.instance.SetActionType(GameState.Coloring);
                StartCoroutine(BeginStencilWork());
            }
        }

        private int _shapeIndexCounter = 0;
        public IEnumerator BeginStencilWork()
        {
            yield return new WaitForSeconds(0.5f);
            CameraController.instance.ChangeCameraForColoring();

            if (takesIndex >= takes.Count)
            {
                for (int i = 0; i < outlinesList.Count; i++)
                {
                    outlinesList[i].sortingOrder = 2;
                    if (i == 0) coloredShapeList[i].sortingOrder = 0;
                     else coloredShapeList[i].sortingOrder = 1;
                }
                yield return new WaitForSeconds(0.5f);
                UIController.instance.winConfetti.SetActive(true);
                yield return new WaitForSeconds(1.5f);
                MainController.instance.SetActionType(GameState.Levelwin);
                yield break;
            }
            currentShapeColor = shapeColors[takesIndex];
            int loopVal = takes[takesIndex++];
            int loopEndVal = _shapeIndexCounter + loopVal;
            for (int i = 0; i < loopVal; i++)
            {
                for (int j = _shapeIndexCounter; j < loopEndVal; j++, _shapeIndexCounter++)
                {
                    ColorShapes.instance.ColorShape(userDrawnShapes[j].points);   
                }
            }
        }
    }
}
