using System;
using System.Collections;
using System.Collections.Generic;
using _Draw_Copy._Scripts.GameplayRelated;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = System.Random;

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

        private PlayerDrawing _playerPen;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            _playerPen = PlayerDrawing.instance;
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
                SoundsController.instance.PlaySound(SoundsController.instance.win);
            }

            if (newState == GameState.Levelfail)
            {
                drawingPercText.transform.parent.gameObject.SetActive(false);
                failPanel.SetActive(true);
                SoundsController.instance.PlaySound(SoundsController.instance.fail);
            }

            if (newState == GameState.RoboDrawing)
            {
                yourTurnStrip.SetActive(false);
                roboTurnStrip.SetActive(true);
                SoundsController.instance.PlaySound(SoundsController.instance.swoosh);
                //_playerPen.enabled = false;
            }
            if (newState == GameState.PlayerDrawing)
            {
                yourTurnStrip.SetActive(true);
                roboTurnStrip.SetActive(false);
                SoundsController.instance.PlaySound(SoundsController.instance.swoosh);
                //_playerPen.enabled = true;
            }
        }

        public IEnumerator ShowMatchPercentage(int min, int max)
        {
            Transform perMatchPanel = drawingPercText.transform.parent;
            perMatchPanel.gameObject.SetActive(true);
            perMatchPanel.transform.DOScaleX(0, 0.3f).From().SetEase(Ease.InOutBack);

            int p = UnityEngine.Random.Range(min, max);
            drawingPercText.DOText(p.ToString(), 2,false, ScrambleMode.Numerals);
            yield return null;
            /*for (int i = 0; i < perc; i++)
            {
                drawingPercText.SetText(i.ToString());
                yield return new WaitForSeconds(0.001f);
            }*/
        }
    }   
}
