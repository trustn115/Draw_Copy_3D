using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LevelCompleteView : MonoBehaviour
{
    public Transform titleParent;
    public Transform button;

    private void OnEnable()
    {
        titleParent.gameObject.SetActive(true);
        titleParent.GetComponent<RectTransform>().DOAnchorPosY(350, 0.75f).From().SetEase(Ease.OutBack);
        DOVirtual.DelayedCall(1f, () =>
        {
            button.gameObject.SetActive(true);
            button.GetComponent<RectTransform>().DOAnchorPosX(-652, 0.75f).From().SetEase(Ease.OutBack); 
        });
    }
}
