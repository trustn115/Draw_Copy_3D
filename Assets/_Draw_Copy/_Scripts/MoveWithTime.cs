using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MoveWithTime : MonoBehaviour
{
    public float startDelay, repeatRate;
    public Vector3 movePos;
    private Vector3 _origiPos;

    private void Start()
    {
        _origiPos = transform.position;
        InvokeRepeating("MovePoint", startDelay, repeatRate);
    }

    void MovePoint()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOLocalMoveX(movePos.x, 0.5f)).Append(transform.DOLocalMoveX(_origiPos.x, 1f));
    }
}
