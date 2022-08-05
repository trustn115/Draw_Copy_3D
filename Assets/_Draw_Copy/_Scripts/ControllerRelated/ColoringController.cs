using System;
using UnityEngine;

namespace  _Draw_Copy._Scripts.ControllerRelated
{
    public class ColoringController : MonoBehaviour
    {
        public static ColoringController instance;
        public Color currentBrushColor;
        public float currentBrushWidth = 0.15f;
        public Color blue, red, yellow, green;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            
        }

        public void ChangeCurrentColor(string colorName)
        {
            switch (colorName)
            {
                case "blue": currentBrushColor = blue;
                    break;
                case "red": currentBrushColor = red;
                    break;
                case "yellow": currentBrushColor = yellow;
                    break;
                case "green": currentBrushColor = green;
                    break;
            }
        }

        public void ChangeBrushWidth(float val)
        {
            currentBrushWidth = val;
        }
    }
}
