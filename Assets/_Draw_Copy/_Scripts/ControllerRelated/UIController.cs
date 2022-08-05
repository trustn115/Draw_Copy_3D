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
        public TextMeshProUGUI levelNumText;
        public GameObject winConfetti;
        public TextMeshProUGUI drawingPercText;
        public GameObject peelHelpButton;
        public RectTransform roboTurnStrip, yourTurnStrip;

        private PlayerDrawing _playerPen;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            _playerPen = PlayerDrawing.instance;
            levelNumText.text = "Lv. " + PlayerPrefs.GetInt("levelnumber", 1);
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
                //winPanel.SetActive(true);
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
                yourTurnStrip.gameObject.SetActive(false);
                roboTurnStrip.gameObject.SetActive(true);
                roboTurnStrip.DOAnchorPosX(roboTurnStrip.anchoredPosition.x + 350, 1).From().SetEase(Ease.OutBack);
                SoundsController.instance.PlaySound(SoundsController.instance.swoosh);
                //_playerPen.enabled = false;
            }
            if (newState == GameState.PlayerDrawing)
            {
                roboTurnStrip.gameObject.SetActive(false);
                yourTurnStrip.gameObject.SetActive(true);
                yourTurnStrip.DOAnchorPosX(roboTurnStrip.anchoredPosition.x + 350, 1).From().SetEase(Ease.OutBack);
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
