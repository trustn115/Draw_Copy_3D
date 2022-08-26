using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onlineornot : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "line")
        {
            GameObject.Find("checker").GetComponent<checker>().counter();
        }
    }
   
}
