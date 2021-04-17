using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //public GameObject hitEffect; //For explosion animation

    public GameObject Source { get; set; }   // entity that fired the bullet

    void OnTriggerEnter2D(Collider2D collision)
    {
        //GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        if (collision.gameObject != Source && (collision.gameObject.layer == 6 || collision.gameObject.layer == 7 || collision.gameObject.layer == 3))
            Destroy(gameObject);
        //Destroy(effect, 5f);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
