using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Draw_Copy._Scripts.ControllerRelated
{
    public class GameController : MonoBehaviour
    {
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
            
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }   
}
