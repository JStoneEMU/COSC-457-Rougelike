using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    public float bulletForce = 2f;

    public void ShootAt(Vector2 relativeLocation)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        if (bulletComponent != null)
        {
            bulletComponent.Source = gameObject;
        }

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(relativeLocation * bulletForce, ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            if (bullet != null && bullet.Source != gameObject)
            {
                Destroy(gameObject);
            }
        }
    }
}