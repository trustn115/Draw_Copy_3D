using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace _Draw_Copy._Scripts.ControllerRelated
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController instance;

        public Camera mainCam;
        public GameObject drawingCamera;
        public GameObject coloringCamera;
        public GameObject finalCamera;

        private void Awake()
        {
            instance = this;
        }

        public void ChangeToWinCamera()
        {
            //mainCam.orthographic = true;
            coloringCamera.SetActive(true);
        }

        public void On_ColoringDoneButtonClicked(RectTransform btnRT)
        {
            coloringCamera.SetActive(false);
            DOVirtual.DelayedCall(0.5f, () => { btnRT.DOAnchorPosX(btnRT.anchoredPosition.x + 300, 0.5f); });
            UIController.instance.winConfetti.SetActive(true);
            UIController.instance.coloringWindow.SetActive(false);
            UIController.instance.brushObject.SetActive(false);
            DOVirtual.DelayedCall(1.5f, () => { CustomerTShirtDrawing.instance.Slap(); });

            if(finalCamera)finalCamera.SetActive(true);
        }
    }   
}
