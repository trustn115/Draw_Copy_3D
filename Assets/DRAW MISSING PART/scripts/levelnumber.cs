using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class levelnumber : MonoBehaviour
{
    private TextMeshProUGUI level;
    private int levelnum;
    void Start()
    {
        level = GetComponent<TextMeshProUGUI>();
        levelnum = SceneManager.GetActiveScene().buildIndex;
        level.text ="LEVEL "+levelnum.ToString();

    }
}
