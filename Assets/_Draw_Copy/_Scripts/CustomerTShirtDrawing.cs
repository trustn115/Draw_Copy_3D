using System;
using System.Collections;
using System.Collections.Generic;
using _Draw_Copy._Scripts.ControllerRelated;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CustomerTShirtDrawing : MonoBehaviour
{
    public Rig rig;

    private float _rigWeight;
    private bool _canLiftHand;
    private float _timer;

    private void Start()
    {
        StartCoroutine(ChangeCameraForDrawing());
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= 1.5f)
        {
            _canLiftHand = true;
        }

        if (_canLiftHand && _rigWeight < 1)
        {
            _rigWeight += Time.deltaTime;
            rig.weight = _rigWeight;
        }
    }

    IEnumerator ChangeCameraForDrawing()
    {
        yield return new WaitForSeconds(3.5f);
        CameraController.instance.coloringCamera.SetActive(true);
        MainController.instance.SetActionType(GameState.PlayerDrawing);
        yield return new WaitForSeconds(5.5f);
        
        //show the done button
        RectTransform doneBtnRT = UIController.instance.tshirtArtDoneButton.GetComponent<RectTransform>();
        doneBtnRT.gameObject.SetActive(true);
        doneBtnRT.DOAnchorPosX(doneBtnRT.anchoredPosition.x + 250, 1).From().SetEase(Ease.OutBack);
    }
}
