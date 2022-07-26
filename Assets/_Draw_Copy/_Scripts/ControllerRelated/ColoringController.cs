using System;
using System.Collections;
using System.Collections.Generic;
using _Draw_Copy._Scripts.GameplayRelated;
using DG.Tweening;
using UnityEngine;

namespace  _Draw_Copy._Scripts.ControllerRelated
{
    public class ColoringController : MonoBehaviour
    {
        public static ColoringController instance;

        public GameObject stencil;
        public GameObject sprayBottle;
        public GameObject peelingHelp;
        public Transform peelingPartsParent;
        public float peelingSpeed;
        private Vector3 _stencilOrigPos;

        private bool _canPeel;
        private float _peelingTime = 0;

        public Color currentShapeColor;

        private void Awake()
        {
            instance = this;
        }

        private Vector3 _peelingParentOrigScale;

        private void Start()
        {
            _peelingParentOrigScale = peelingPartsParent.localScale;
        }

        private void Update()
        {
            if (Input.GetMouseButton(0) && _canPeel)
            {
                
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
                    stencil.SetActive(false);
                }
                
            }

            if (Input.GetMouseButtonUp(0) && _canPeel)
            {
                if (peelingPartsParent.localScale.x < 53)
                {
                    peelingPartsParent.DOScale(_peelingParentOrigScale, 1);
                }
            }
        }

        public IEnumerator MoveStencilForColoring(Vector3 stencilMovePos, List<Transform> drawnPoints)
        {
            _stencilOrigPos = stencil.transform.position;
            stencil.SetActive(true);
            stencil.transform.DOMove(stencilMovePos, 1);
            
            yield return new WaitForSeconds(2);
            //show the spray bottle & spray help
            sprayBottle.SetActive(true);
            sprayBottle.GetComponent<SprayColoring>().targetNumOfPointsToReach = drawnPoints.Count;
            sprayBottle.transform.DOMove(stencilMovePos, 1);
            //calculate how many points touched - if more than 60%
            
            //show the peeling help
            //peel on mousedown
            //disable stencil after peeling
            //READYYY
            //stencil.transform.DOMove(_stencilOrigPos, 1);
        }

        public void MinimumColoringReached()
        {
            sprayBottle.SetActive(false);
            peelingHelp.SetActive(true);
            UIController.instance.peelHelpButton.SetActive(true);
        }

        public void OnPeelingHelpClicked()
        {
            peelingHelp.SetActive(false);
            _canPeel = true;
            LineRenderer filledColor = GetComponent<ColorShapes>().filledColor;
            filledColor.startColor = currentShapeColor;
            filledColor.endColor = currentShapeColor;
        }
    }
}
