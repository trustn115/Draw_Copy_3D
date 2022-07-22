using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LevelFailView : MonoBehaviour
{
    public Transform titleParent;
    public Transform button;

    private void OnEnable()
    {
        titleParent.gameObject.SetActive(true);
        titleParent.GetComponent<RectTransform>().DOAnchorPosY(350, 1f).From().SetEase(Ease.OutBack);
        DOVirtual.DelayedCall(1.2f, () =>
        {
            button.gameObject.SetActive(true);
            button.GetComponent<RectTransform>().DOAnchorPosX(-652, 1f).From().SetEase(Ease.OutBack); 
        });
    }
}
