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

        public GameObject winPanel, failPanel;
        public GameObject winConfetti;
        public TextMeshProUGUI drawingPercText;

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
                winPanel.SetActive(true);
            }

            if (newState == GameState.Levelfail)
            {
                drawingPercText.transform.parent.gameObject.SetActive(false);
                failPanel.SetActive(true);
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
