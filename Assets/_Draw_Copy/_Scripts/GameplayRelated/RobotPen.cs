using System;
using System.Collections;
using System.Collections.Generic;
using _Draw_Copy._Scripts.ControllerRelated;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Draw_Copy._Scripts.GameplayRelated
{
    public class RobotPen : MonoBehaviour
    {
        private LineRenderer _line;
        public List<Transform> points;
        
        private void Start()
        {
            _line = GetComponent<LineRenderer>();
            Vector3 initialLinePos =  new Vector3(transform.position.x, -1, transform.position.z);
            _line.SetPosition(0, points[0].position);
            _line.SetPosition(1, initialLinePos);
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
            if(newState==GameState.RoboDrawing)
            {
                StartCoroutine(FormTheShape());
            }
            
        }
        private void Update()
        {
            _line.positionCount++;
            _line.SetPosition(_line.positionCount - 1, new Vector3(transform.position.x, -1, transform.position.z) );

            if (Input.GetMouseButtonDown(1))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        IEnumerator FormTheShape()
        {
            for (int i = 0; i < points.Count; i++)
            {
                transform.DOMove(new Vector3(points[i].position.x, transform.position.y, points[i].position.z), 0.12f).SetEase(Ease.Linear);
                yield return new WaitForSeconds(0.12f);
            }
            MainController.instance.SetActionType(GameState.PlayerDrawing);
        }
    }   
}
