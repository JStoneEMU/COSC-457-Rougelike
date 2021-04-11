using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    public float moveSpeed = 5f;
    public float maxHealth = 10;
    public float bulletForce = 2f;

    private Rigidbody2D rb;
    private float currentHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void MoveTowards(Vector2 point, float deltaTime)
    {
        Vector2 position = transform.position;
        position = Vector2.MoveTowards(position, point, moveSpeed * deltaTime);
        rb.MovePosition(position);
    }

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
                currentHealth -= 10;
            }
        }
    }
}