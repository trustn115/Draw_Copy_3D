using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelloadder : MonoBehaviour
{
   
    void Start()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("level",1));
        Debug.Log(PlayerPrefs.GetInt("level"));
    }
    void Update()
    {  
    }
}
