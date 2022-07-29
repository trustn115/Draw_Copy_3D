using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Draw_Copy._Scripts.ControllerRelated
{
    public class UIController : MonoBehaviour
    {
        public static UIController instance;

        public GameObject HUD;
        public GameObject winPanel, failPanel;
        public GameObject winConfetti;
        public TextMeshProUGUI drawingPercText;
        public GameObject peelHelpButton;
        public GameObject roboTurnStrip, yourTurnStrip;

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
            if(newState==GameState.Levelwin)
            {
                HUD.SetActive(false);
                winPanel.SetActive(true);
            }

            if (newState == GameState.Levelfail)
            {
                drawingPercText.transform.parent.gameObject.SetActive(false);
                HUD.SetActive(false);
                failPanel.SetActive(true);
            }

            if (newState == GameState.RoboDrawing)
            {
                yourTurnStrip.SetActive(false);
                roboTurnStrip.SetActive(true);
            }
            if (newState == GameState.PlayerDrawing)
            {
                yourTurnStrip.SetActive(true);
                roboTurnStrip.SetActive(false);
            }
        }

        public IEnumerator ShowMatchPercentage(int perc)
        {
            Transform perMatchPanel = drawingPercText.transform.parent;
            perMatchPanel.gameObject.SetActive(true);
            perMatchPanel.transform.DOScaleX(0, 0.3f).From().SetEase(Ease.InOutBack);
            for (int i = 0; i < perc; i++)
            {
                drawingPercText.SetText(i.ToString());
                yield return new WaitForSeconds(0.001f);
            }
        }
    }   
}
