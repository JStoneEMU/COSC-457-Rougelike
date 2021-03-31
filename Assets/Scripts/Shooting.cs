using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public bool takeInput = false;
    public Transform firePoint;
    public GameObject bulletPrefab;

    public float bulletForce = 20f;

    // Update is called once per frame
    void Update()
    {
        if(takeInput && Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

        void Shoot()
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce((firePoint.up * -1) * bulletForce, ForceMode2D.Impulse);
        }
    
    }

    public void ShootAt(Vector2 relativeLocation)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(relativeLocation * bulletForce, ForceMode2D.Impulse);
    }
}
