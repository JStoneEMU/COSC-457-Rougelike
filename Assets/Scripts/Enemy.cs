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

    // Object being looked at, if null rotation is based on movement
    public GameObject LookingAt { get; set; }

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

        if (LookingAt != null)
        {
            Vector2 lookPos = LookingAt.transform.position;
            Vector2 lookDir = lookPos - rb.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + 0f;
            rb.rotation = angle;
        }
    }

    public void MoveTowards(Vector2 point, float deltaTime)
    {
        if (LookingAt == null)
        {
            Vector2 lookDir = point - rb.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + 0f;
            rb.rotation = angle;
        }

        Vector2 position = transform.position;
        position = Vector2.MoveTowards(position, point, moveSpeed * deltaTime);
        rb.MovePosition(position);       
    }

    public void ShootAt(Vector2 relativeLocation)
    {
        if (LookingAt == null)
        {
            Vector2 lookDir = relativeLocation;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + 0f;
            rb.rotation = angle;
        }

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        if (bulletComponent != null)
        {
            bulletComponent.Source = gameObject;
        }

        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.AddForce(relativeLocation * bulletForce, ForceMode2D.Impulse);
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