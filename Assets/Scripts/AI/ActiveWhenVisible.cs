using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWhenVisible : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
    }

    void OnBecameVisible()
    {
        gameObject.SetActive(true);
        print("became visible!");
    }

    void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
