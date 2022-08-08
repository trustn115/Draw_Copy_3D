using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Draw_Copy._Scripts.ElementRelated
{
    public class BrushTypesElement : MonoBehaviour
    {
        public static BrushTypesElement instance;

        public List<GameObject> brushObjects;

        private void Awake()
        {
            instance = this;
        }

        public void ChangeBrush(int i)
        {
            for (int j = 0; j < brushObjects.Count; j++)
            {
                brushObjects[j].SetActive(false);
            }
            brushObjects[i].SetActive(true);
        }
    }   
}
