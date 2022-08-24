using System;
using System.Collections.Generic;
using _Draw_Copy._Scripts.ElementRelated;
using UnityEngine;
using UnityEngine.Rendering;

namespace  _Draw_Copy._Scripts.ControllerRelated
{
    public class ColoringController : MonoBehaviour
    {
        public static ColoringController instance;

        [Header("BRUSH RELATED")]
        public GameObject currentBrush;
        public GameObject defaultBrush;
        public GameObject eraserBrush;
        private int _sortingOrder = 1;
        public float currentBrushWidth = 0.15f;
        public Color grey,brown,purple,blue, red, pink,yellow, green;
        Gradient _gradient = new Gradient();

        private GameObject _brushParent;
        
        //TODO: Variables
        private List<GameObject> _brushesList= new List<GameObject>();
        private List<GameObject> _undoBrushes = new List<GameObject>();
        public List<GameObject> selectedRingsList;

        public SpriteRenderer brush;
        public List<Sprite> brushSprites;

        public LineRenderer eraser;
        public Color eraserColor;

        public BrushTypesElement brushTypesElement;
        public bool isCanvasLevel;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            SetUpColor(Color.grey);
            currentBrush = defaultBrush;
            _brushParent = new GameObject("BrushParent");
        }

        public LineRenderer SetupBrush()
        {
            GameObject b = Instantiate(currentBrush);
            LineRenderer brush = b.GetComponent<LineRenderer>();
            _brushesList.Add(b);
            brush.colorGradient = _gradient;
            
            brush.startWidth =currentBrushWidth;
            brush.endWidth = currentBrushWidth;
            brush.sortingOrder = _sortingOrder++;
            b.transform.parent = _brushParent.transform;
            return brush;
        }

        public void ChangeCurrentColor(string colorName)
        {
            DisableAllSelectedRings();
            switch (colorName)
            {
                case "grey": SetUpColor(grey);
                    break;
                case "brown": SetUpColor(brown);
                    break;
                case "purple": SetUpColor(purple);
                    break;
                case "blue": SetUpColor(blue);
                    break;
                case "red": SetUpColor(red);
                    break;
                case "pink": SetUpColor(pink);
                    break;
                case "yellow": SetUpColor(yellow);;
                    break;
                case "green": SetUpColor(green);
                    break;
                case "rainbow": SetUpRainbowColor();
                    break;
            }
            SoundsController.instance.PlaySound(SoundsController.instance.buttonPop);
            Vibration.Vibrate(27);
        }

        private bool _canChangeColor = true;
        void SetUpColor(Color c)
        {
            if(!_canChangeColor) return;
            _gradient = new Gradient();
            _gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(c, 0.0f), new GradientColorKey(c, 1.0f), },
                new GradientAlphaKey[] { new GradientAlphaKey(1, 0.0f), new GradientAlphaKey(1, 1.0f) }
            );
        }
        void SetUpRainbowColor()
        {
            _gradient = new Gradient();
            _gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f),
                    new GradientColorKey(Color.magenta, 0.2f),
                    new GradientColorKey(Color.yellow, 0.4f),
                    new GradientColorKey(Color.cyan, 0.6f),
                    new GradientColorKey(Color.green, 0.8f),
                    new GradientColorKey(Color.blue, 1f),
                },
                new GradientAlphaKey[] { new GradientAlphaKey(1, 0.0f), new GradientAlphaKey(1, 1.0f) }
            );
        }

        public void ChangeBrush(int i)
        {
            brushTypesElement.ChangeBrush(i);
            switch (i)
            {
                case 0: currentBrushWidth = 0.15f;
                    break;
                case 1: currentBrushWidth = 0.3f;
                    break;
                case 2: currentBrushWidth = 0.5f;
                    break;
            }
            SoundsController.instance.PlaySound(SoundsController.instance.buttonBrushPop);
            Vibration.Vibrate(27);
            /*if (i == brushes.Count - 1)
            {
                currentBrush = eraserBrush;
                _canChangeColor = true;
                SetUpColor(eraserColor);
                _canChangeColor = false;
            }
            else
            {
                currentBrush = defaultBrush;
                _canChangeColor = true;
            }*/
        }
        public void ChangeBrushWidth(float val)
        {
            currentBrushWidth = val;
        }

        public void OnUndoButtonPressed()
        {
            for (int i = _brushesList.Count - 1; i >= 0; i--)
            {
                GameObject line = _brushesList[i];
                if (line.activeInHierarchy)
                {
                    line.SetActive(false);
                    _undoBrushes.Add(line);
                    break;
                }
            }
            Vibration.Vibrate(27);
        }
        public void OnRedoButtonPressed()
        {
            for (int i = 0; i <_undoBrushes.Count; i++)
            {
                GameObject line = _undoBrushes[i];
                if (!line.activeInHierarchy)
                {
                    line.SetActive(true);
                    _brushesList.Add(line);
                    break;
                }
            }
            Vibration.Vibrate(27);
        }
        public void OnClearButtonPressed()
        {
            _brushParent.SetActive(false);
            _brushParent = new GameObject("Brush Parent");
            Vibration.Vibrate(27);
        }
        
        void DisableAllSelectedRings()
        {
            for (int i = 0; i < selectedRingsList.Count; i++)
            {
                selectedRingsList[i].SetActive(false);
            }
        }
    }
}
