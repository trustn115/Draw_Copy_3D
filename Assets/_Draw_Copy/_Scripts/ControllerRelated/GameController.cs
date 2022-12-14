using System;
using System.Collections;
using System.Collections.Generic;
using _Draw_Copy._Scripts.GameplayRelated;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Draw_Copy._Scripts.ControllerRelated
{
    public class GameController : MonoBehaviour
    {
        public static GameController instance;
        public List<GameObject> thingsToDeactivateDuringColoring;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            Vibration.Init();
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
            if(newState==GameState.Levelstart)
            {
                
            }

            if (newState == GameState.Coloring)
            {
                for (int i = 0; i < thingsToDeactivateDuringColoring.Count; i++)
                {
                    thingsToDeactivateDuringColoring[i].SetActive(false);
                }
            }
            
        }

        private void Update()
        {
             if(Input.GetMouseButtonDown(1))
             {
                 On_RetryButtonClicked();
             }
        }

        public void On_NextButtonClicked()
        {
            if (PlayerPrefs.GetInt("level", 1) >= SceneManager.sceneCountInBuildSettings - 1)
            {
                SceneManager.LoadScene(UnityEngine.Random.Range(0, SceneManager.sceneCountInBuildSettings - 1));
                PlayerPrefs.SetInt("level", (PlayerPrefs.GetInt("level", 1) + 1));
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                PlayerPrefs.SetInt("level", (PlayerPrefs.GetInt("level", 1) + 1));
            }
            PlayerPrefs.SetInt("levelnumber", PlayerPrefs.GetInt("levelnumber", 1) + 1);
            Vibration.Vibrate(27);
        }

        public void On_RetryButtonClicked()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Vibration.Vibrate(27);
        }
        
        [HideInInspector] public int playerTurnCounter;
        public void ChangeToRoboDrawingState()
        {
            playerTurnCounter++;
            if(PlayerDrawing.instance)
                PlayerDrawing.instance.CheckIfAllShapesDrawn(playerTurnCounter);
            else PlayerDrawingGirlBack.instance.CheckIfAllShapesDrawn(playerTurnCounter);
            Vibration.Vibrate(27);
        }
    }   
}
