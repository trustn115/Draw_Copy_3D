using System;
using System.Collections;
using System.Collections.Generic;
using _Draw_Copy._Scripts.ControllerRelated;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class GirlDrawingBackElement : MonoBehaviour
{
    private Animator _animator;
    public Transform movePos;
    private RigBuilder _rigBuilder;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigBuilder = GetComponent<RigBuilder>();
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
            StartCoroutine(MoveAndClp());    
        }
            
    }
    IEnumerator MoveAndClp()
    {
        _animator.SetTrigger("walk");
        _rigBuilder.enabled = false;
        transform.DOMove(movePos.position, 2);
        transform.DOLocalRotate(Vector3.up * 180, 2);
        
        yield return new WaitForSeconds(2);
        _animator.SetTrigger("clap");
        yield return new WaitForSeconds(3.5f);
        gameObject.SetActive(false);
    }
}
