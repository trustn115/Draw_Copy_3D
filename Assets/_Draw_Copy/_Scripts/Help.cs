using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Help : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.GetInt("levelnumber", 1) == 1)
        {
            transform.GetChild(2).gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameObject.SetActive(false);
        }
    }
}
