using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hint : MonoBehaviour
{
    public SpriteRenderer[] points;
    public bool onoroff;
    public void Start()
    {
        for (int i = 0; i < points.Length; i++)
        {
            points[i].enabled = false;
        }
    }
    public void hinton()
    {
        for (int i = 0; i < points.Length; i++)
        {
            points[i].enabled = true;
        }
        onoroff = true;
    }
    public void turnoffagain()
    {
        if (onoroff)
        {
            for (int i = 0; i < points.Length; i++)
            {
                points[i].enabled = false;
            }
        }
    }
}
