using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    
    Vector2 movement;

    public Camera cam;

    Vector2 mousePos;

    public int maxHealth = 5;
    public int currentHealth;

    public HealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update() //Put all input in here
    {
        //if(Input.GetKeyDown(KeyCode.Space))
      //  {
      //      TakeDamage(1);
      //  }
        movement.x = Input.GetAxisRaw("Horizontal"); //Left input gives -1, right givs 1, nothing gives 0
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void setSpeed(float speed)
    {
        moveSpeed = speed;
    }
    void FixedUpdate() 
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);//Time.fixedDeltaTime ensures our movement speed is consistent no matter how many times FixedUpdate is called

        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + 90f;
        rb.rotation = angle;
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            TakeDamage(1);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) //This doodad is for picking up items
    {
        if (other.gameObject.CompareTag("SpeedPowerup"))
        {
            setSpeed(10f);
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("HealthPack"))
        {
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
            other.gameObject.SetActive(false);
        }
    }
}
