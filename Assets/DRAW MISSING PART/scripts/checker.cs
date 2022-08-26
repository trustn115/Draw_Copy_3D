using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class checker : MonoBehaviour
{
    public int totalpoint;
    public int presentcount;
    public GameObject first, second;
    public GameObject stars;
    public GameObject hintbutton, nextlevelbutton;
    public void Start()
    {
        presentcount = 0;
        hintbutton.SetActive(true);
        nextlevelbutton.SetActive(false);
        first.SetActive(true);
        second.SetActive(false);
        stars.SetActive(false);
    }
    public void counter()
    {
        presentcount++;
    }
    public void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            if (totalpoint == presentcount)
            {
                first.SetActive(false);
                second.SetActive(true);
                stars.SetActive(true);
                hintbutton.SetActive(false);
                nextlevelbutton.SetActive(true);
                gameObject.GetComponent<hint>().turnoffagain();
                PlayerPrefs.SetInt("level", SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                presentcount = 0;
            }
        }
    }
    public void wrongdrawing()
    {
        presentcount-=10;
    }
}
