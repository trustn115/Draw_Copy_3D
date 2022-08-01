using System;
using System.Collections;
using System.Collections.Generic;
using _Draw_Copy._Scripts.ControllerRelated;
using DG.Tweening;
using UnityEngine;

namespace _Draw_Copy._Scripts.GameplayRelated
{
    public class SprayColoring : MonoBehaviour
    {
        public static SprayColoring instance;
        
        [HideInInspector] public bool canSpray;
        [HideInInspector] public LineRenderer currentLine;
        private Vector3 _lastPos;
        public GameObject brush;

        public int targetNumOfPointsToReach;
        private Collider _collider;
        public ParticleSystem sprayFx;

        private void Awake()
        {
            instance = this;
        }

        private void OnEnable()
        {
            _collider = GetComponent<Collider>();
            _collider.enabled = false;
        }

        private void Start()
        {
            canSpray = true;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && canSpray)
            {
                CreateBrush();
                _collider.enabled = true;
                sprayFx.Play();
            }

            if (Input.GetMouseButton(0) && canSpray)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 10))
                {
                    Vector3 hitPos = hit.point;
                    transform.position = new Vector3(hitPos.x, transform.position.y, hitPos.z);
                    if (hitPos != _lastPos && Vector3.Distance(hitPos, _lastPos) > 0.05f)
                    {
                        AddPoint(new Vector3(hitPos.x, transform.localPosition.y, hitPos.z));
                        _lastPos = hitPos;
                    }
                }
            }

            if (Input.GetMouseButtonUp(0) && canSpray)
            {
                _collider.enabled = false;
                sprayFx.Stop();
            }
        }

        [SerializeField] private int _pointsReachCounter = 0;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 6)
            {
                other.gameObject.layer = 0;
                other.GetComponent<Collider>().enabled = false;
                _pointsReachCounter++;
                if (_pointsReachCounter > (int)(targetNumOfPointsToReach / 1.3f))
                {
                    StencilController.instance.MinimumColoringReached();
                    _pointsReachCounter = 0;
                }
            }
        }

        void AddPoint(Vector3 pointPos)
        {
            currentLine.positionCount++;
            int positionIndex = currentLine.positionCount - 1;
            currentLine.SetPosition(positionIndex, pointPos);
        }

        void CreateBrush()
        {
            currentLine = Instantiate(brush).GetComponent<LineRenderer>();
            currentLine.transform.parent = StencilController.instance.stencil.transform;
            currentLine.enabled = true;
            
            DOVirtual.DelayedCall(0.05f, () =>
            {
                currentLine.SetPosition(0, new Vector3(transform.position.x, -1, transform.position.z));
                currentLine.SetPosition(1, new Vector3(transform.position.x, -1, transform.position.z));
            });
        }

        public void DisableBrushAfterRound()
        {
            currentLine.positionCount = 2;
            currentLine.enabled = false;
        }
    }
}