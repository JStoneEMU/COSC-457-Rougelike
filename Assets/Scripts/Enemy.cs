using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject healthBarObj;

    public float moveSpeed = 5f;
    public int maxHealth = 10;
    public float bulletForce = 2f;
    public float healthBarDuration = 2f;

    // Currently only used for minimax search
    public int attackRange = 10;
    public int attackDamage = 10;

    public int CurrentHealth { get; set; }
    // Object being looked at, if null rotation is based on movement
    public GameObject LookingAt { get; set; }

    private Rigidbody2D rb;
    private float initialRotation;
    private HealthBar healthBarScript;
    private float healthBarTimer = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        CurrentHealth = maxHealth;
        transform.rotation.ToAngleAxis(out initialRotation, out var axis);
        healthBarScript = healthBarObj.GetComponent<HealthBar>();
        healthBarScript.SetMaxHealth(maxHealth);
        if (healthBarDuration > 0)
            HideHealthBar();
    }

    void Update()
    {
        if (CurrentHealth <= 0)
        {
            Destroy(gameObject);
        }

        if (LookingAt != null)
        {
            Vector2 lookPos = LookingAt.transform.position;
            Vector2 lookDir = lookPos - rb.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + 0f;
            rb.rotation = initialRotation + angle;
        }

        if (healthBarTimer > 0)
        {
            healthBarTimer -= Time.deltaTime;
        }
        else
        {
            HideHealthBar();
        }
    }

    public void MoveTowards(Vector2 point, float deltaTime)
    {
        if (LookingAt == null)
        {
            Vector2 lookDir = point - rb.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + 0f;
            rb.rotation = initialRotation + angle;
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
            rb.rotation = initialRotation + angle;
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
                CurrentHealth -= 10;
                ShowHealthBar();
            }
        }
    }

    private void ShowHealthBar()
    {
        healthBarObj.SetActive(true);
        healthBarScript.SetHealth(CurrentHealth);
        healthBarTimer = healthBarDuration;
    }

    private void HideHealthBar()
    {
        healthBarObj.SetActive(false);
    }
}