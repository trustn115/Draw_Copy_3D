using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using Tabtale.TTPlugins;

public class StartScreen : MonoBehaviour
{
    private void Awake()
    {
        TTPCore.Setup();
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("level", 1) > SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(Random.Range(0, SceneManager.sceneCountInBuildSettings - 1));
        }
        else
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt("level", 1));
        }
    }
}
