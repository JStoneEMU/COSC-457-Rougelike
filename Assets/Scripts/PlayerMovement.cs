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
    public GameObject deathScreen;

    public float invincibilityTimeOnHit = 1;

    private float invincibilityTimer = 0;

    public Animator animator;

    public GameObject character;

    public AudioSource pistol;
    public AudioSource reload;
    public AudioSource heal;
    public AudioSource empty;
    public AudioSource hurt;
    public AudioSource nvg;


    public GameObject nightVision;

    public RuntimeAnimatorController anim1;
    public RuntimeAnimatorController anim2;
    public RuntimeAnimatorController anim3;

    public int AKInt = 0;
    public int NVGCheck;
    public int ShotgunCheck;
    public int AKCheck;


    void Start()
    {
        this.GetComponent<Animator>().runtimeAnimatorController = anim1 as RuntimeAnimatorController;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        /*AudioSource[] aSources = GetComponents<AudioSource>();
        pistol = aSources[0];
        reload = aSources[1];
        heal = aSources[2];*/
        NVGCheck = PlayerPrefs.GetInt("HasNVG", 0);
        ShotgunCheck = PlayerPrefs.GetInt("HasShotgun", 0);
        AKCheck = PlayerPrefs.GetInt("HasAK", 0);

        if (NVGCheck == 1)
            {
            setNVG();
            }
        if (ShotgunCheck == 1)
            { 
            setShotgun(); 
            }
        if (AKCheck == 1)
            {
            setAK();
            }
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

        animator.SetFloat("Moving", movement.sqrMagnitude);

        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.Play("Player_Reload");
        }
        if (Input.GetButtonDown("Fire1"))
        {  
        animator.Play("Player_Shoot");   
        }

        if (invincibilityTimer > 0)
            invincibilityTimer -= Time.deltaTime;

        if (currentHealth <= 0)
        {
            Death();
           
        }
    }

    void setSpeed(float speed)
    {
        moveSpeed = speed;
    }
    void FixedUpdate() 
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);//Time.fixedDeltaTime ensures our movement speed is consistent no matter how many times FixedUpdate is called

        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + 0f;
        rb.rotation = angle;
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        hurt.Play();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "SmartEnemy")
        {
            if (invincibilityTimer <= 0)
            {
                Enemy enemyComponent = other.gameObject.GetComponent<Enemy>();
                if (enemyComponent)
                {
                    // enemy damage/health is in multiples of 10 for no good reason, so divided by 10
                    TakeDamage(enemyComponent.attackDamage / 10);
                    invincibilityTimer = invincibilityTimeOnHit;
                }
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        //This doodad is for picking up items
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
            heal.Play();
        }

        // Take damage when hit by bullet
        if (other.gameObject.tag == "Bullet")
        {
            TakeDamage(1);
            hurt.Play();
        }

/*        if (other.gameObject.tag == "Finish")
        {
            SceneManager.LoadScene("SampleScene");
        }*/

        if (other.gameObject.tag == "NVG")
        {
            other.gameObject.SetActive(false);
            setNVG();
            nvg.Play();
        }
        if (other.gameObject.tag == "AK")
        {
            other.gameObject.SetActive(false);
            setAK();
        }
        if (other.gameObject.tag == "Shotgun")
        {
            other.gameObject.SetActive(false);
            setShotgun();
        }
    }
    private void setAK()
    {
        this.GetComponent<Animator>().runtimeAnimatorController = anim2 as RuntimeAnimatorController;
        character.GetComponent<AKShooting>().enabled = true;
        character.GetComponent<Shooting>().enabled = false;
        character.GetComponent<ShotgunShooting>().enabled = false;
        AKInt = 1;
        PlayerPrefs.SetInt("HasAK", 1);
    }
    private void setShotgun()
    {
        this.GetComponent<Animator>().runtimeAnimatorController = anim3 as RuntimeAnimatorController;
        character.GetComponent<ShotgunShooting>().enabled = true;
        character.GetComponent<Shooting>().enabled = false;
        PlayerPrefs.SetInt("HasShotgun", 1);
    }
    private void setNVG()
    {
        nightVision.SetActive(true);
        PlayerPrefs.SetInt("HasNVG", 1);
    }

    private void Death()
    {
        //string sceneName = SceneManager.GetActiveScene().name;
        //SceneManager.LoadScene(sceneName);

        deathScreen.SetActive(true);
        gameObject.SetActive(false);
        
    }

}
