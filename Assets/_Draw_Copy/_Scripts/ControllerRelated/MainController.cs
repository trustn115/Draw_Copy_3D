using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace _Draw_Copy._Scripts.ControllerRelated
{
    public enum GameState
    {
        None,
        Create,
        Levelstart,
        RoboDrawing,
        PlayerDrawing,
        Levelwin,
        Levelfail
    }

    public class MainController : MonoBehaviour
    {
        public static MainController instance;
        
        [SerializeField] private GameState _gameState;
        public static event System.Action<GameState, GameState> GamestateChanged;

        public GameState GameState
        {
            get => _gameState;
            private set
            {
                if (value != _gameState)
                {
                    GameState oldState = _gameState;
                    _gameState = value;
                    if (GamestateChanged != null)
                        GamestateChanged(_gameState, oldState);
                }
            }
        }

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            CreateGame();
        }

        void CreateGame()
        {
            GameState = GameState.Create;
        }

        public void SetActionType(GameState _curState)
        {
            GameState = _curState;
        }
    }
}