using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Draw_Copy._Scripts.ControllerRelated
{
    public class SoundsController : MonoBehaviour
    {
        public static SoundsController instance;

        public AudioSource mainAudioSource, roboDrawSource, playerDrawSource;
        public AudioClip confetti, win, fail, swoosh, paintBrush, buttonPop, buttonClick, buttonBrushPop;

        private void Awake()
        {
            instance = this;
        }

        public void PlaySound(AudioClip clip)
        {
            mainAudioSource.PlayOneShot(clip);
        }
    }   
}
