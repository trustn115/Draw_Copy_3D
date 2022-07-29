using System;
using System.Collections;
using System.Collections.Generic;
using _Draw_Copy._Scripts.GameplayRelated;
using DG.Tweening;
using UnityEngine;

namespace _Draw_Copy._Scripts.ControllerRelated
{
    public class StencilController : MonoBehaviour
    {
        public static StencilController instance;
        
        [Header("STENCIL RELATED")]
        public GameObject stencil;
        public GameObject sprayBottle;
        public GameObject peelingHelp;
        public Transform peelingPartsParent;
        public Transform finalPeelingPart, finalFoldingPart;
        public float peelingSpeed;

        private bool _canPeel;
        private float _peelingTime = 0;

        [Header("ORIGINAL STATES")]
        private Vector3 _stencilOrigPos, _peelingPartsOrigScale, _finalPeelingPartOrigScale, _finalFoldingPartOrigPos;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            _peelingPartsOrigScale = peelingPartsParent.localScale;
            _finalPeelingPartOrigScale = finalPeelingPart.localScale;
            _finalFoldingPartOrigPos = finalFoldingPart.position;
        }
        
        public IEnumerator MoveStencilForColoring(Vector3 stencilMovePos, List<Transform> drawnPoints)
        {
            _stencilOrigPos = stencil.transform.position;
            stencil.SetActive(true);
            stencil.transform.DOMove(stencilMovePos, 1).OnComplete(() =>
            {
                sprayBottle.transform.DOMove(stencilMovePos, 1);
            });
            sprayBottle.SetActive(true);
            sprayBottle.GetComponent<SprayColoring>().targetNumOfPointsToReach = drawnPoints.Count;
            //show the spray bottle & spray help
            yield return null;
        }
        private bool _peelingActivated;
        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && _peelingActivated)
            {
                peelingHelp.SetActive(false);
                _canPeel = true;
                _peelingActivated = false;
                peelingPartsParent.gameObject.SetActive(true);
            }
            if (Input.GetMouseButton(0) && _canPeel)
            {
                if (peelingPartsParent.localScale.x < 53)
                {
                    peelingPartsParent.localScale += Vector3.one * peelingSpeed * Time.deltaTime;
                }
                else
                {
                    _canPeel = false;
                    PeelCompletely();
                }
            }

            if (Input.GetMouseButtonUp(0) && _canPeel)
            {
                if (peelingPartsParent.localScale.x < 53)
                {
                    peelingPartsParent.DOScale(_peelingPartsOrigScale, 1);
                }
            }
        }
        void PeelCompletely()
        {
            peelingPartsParent.gameObject.SetActive(false);
            finalFoldingPart.gameObject.SetActive(true);
            finalPeelingPart.gameObject.SetActive(true);
            
            LineRenderer filledColor = ColorShapes.instance.fill;
            filledColor.startColor = ColoringController.instance.currentShapeColor;
            filledColor.endColor = ColoringController.instance.currentShapeColor;
            finalPeelingPart.DOScaleY(2.05f, 1);
            finalFoldingPart.DOLocalMoveY(-6.35f,1).OnComplete(() =>
            {
                stencil.SetActive(false);
                ResetStencilState();
                SprayColoring.instance.DisableBrushAfterRound();
                ColoringController.instance.StartCoroutine(ColoringController.instance.BeginStencilWork());
            });
        }
        public void MinimumColoringReached()
        {
            print("Min coloring reached!");
            sprayBottle.SetActive(false);
            peelingHelp.SetActive(true);
            _peelingActivated = true;
            //UIController.instance.peelHelpButton.SetActive(true);
        }

        void ResetStencilState()
        {
            stencil.transform.position = _stencilOrigPos;
            peelingPartsParent.localScale = _peelingPartsOrigScale;
            finalPeelingPart.localScale = _finalPeelingPartOrigScale;
            finalFoldingPart.position = _finalFoldingPartOrigPos;
            finalFoldingPart.gameObject.SetActive(false);
            finalPeelingPart.gameObject.SetActive(false);
            _peelingActivated = false;
        }
    }   
}
