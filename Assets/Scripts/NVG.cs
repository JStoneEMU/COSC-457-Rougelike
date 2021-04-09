using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NVG : MonoBehaviour
{
    public GameObject nightVision;


    void OnTriggerEnter2d(Collider2D other)
    {
       if (other.gameObject.tag == "Player")
            nightVision.SetActive(true);
    }
}