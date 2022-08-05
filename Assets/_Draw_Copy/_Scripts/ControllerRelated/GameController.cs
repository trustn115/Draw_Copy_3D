using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Draw_Copy._Scripts.ControllerRelated
{
    public class GameController : MonoBehaviour
    {
        public List<GameObject> thingsToDeactivateDuringColoring;
        
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
        }

        public void On_RetryButtonClicked()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }   
}
