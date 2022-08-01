using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Draw_Copy._Scripts.ControllerRelated
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController instance;

        public Camera mainCam;
        public GameObject drawingCamera;
        public GameObject coloringCamera;

        private void Awake()
        {
            instance = this;
        }

        public void ChangeToWinCamera()
        {
            //mainCam.orthographic = true;
            coloringCamera.SetActive(true);
        }
    }   
}
