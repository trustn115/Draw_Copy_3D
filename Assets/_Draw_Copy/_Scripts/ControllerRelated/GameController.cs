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
            
        }

        public void NextLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void RetryLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }   
}
