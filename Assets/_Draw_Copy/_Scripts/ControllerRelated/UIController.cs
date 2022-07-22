using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Draw_Copy._Scripts.ControllerRelated
{
    public class UIController : MonoBehaviour
    {
        public static UIController instance;

        public GameObject winPanel, failPanel;

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
                failPanel.SetActive(true);
            }
            
        }
    }   
}
