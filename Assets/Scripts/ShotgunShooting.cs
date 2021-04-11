using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShotgunShooting : MonoBehaviour
{
    public bool takeInput = false;
    public Transform firePoint;
    public Transform firePoint2;
    public Transform firePoint3;
    public GameObject bulletPrefab;

    public TextMeshProUGUI magDisplay;
    public int magStatus = 4;

    public float bulletForce = 20f;

    private AudioSource pistol;
    private AudioSource reload;
    private AudioSource heal;
    private AudioSource shotgun;


    void Start()
    {
        AudioSource[] aSources = GetComponents<AudioSource>();
        pistol = aSources[0];
        reload = aSources[1];
        heal = aSources[2];
        shotgun = aSources[3];
    }

    // Update is called once per frame
    void Update()
    {
        magDisplay.text = magStatus + "/4";
        if (takeInput && Input.GetButtonDown("Fire1"))
        {
            Shoot();
            shotgun.Play();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            reload.Play();
            if (magStatus > 0) //Load 1 in chamber plus full mag
            {
                magStatus = 5;

            }
            if (magStatus == 0) //Load full mag
            {
                magStatus = 4;
            }
            if (magStatus == 5)//Swap full mag for another full mag
            {
                //Do nothing
            }
        }
    }
    void Shoot()
    {
        if (magStatus > 0)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Bullet bulletComponent = bullet.GetComponent<Bullet>();
            if (bulletComponent != null)
            {
                bulletComponent.Source = gameObject;
            }
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce((firePoint.up * -1) * bulletForce, ForceMode2D.Impulse);

            GameObject bullet2 = Instantiate(bulletPrefab, firePoint2.position, firePoint2.rotation);
            Bullet bulletComponent2 = bullet.GetComponent<Bullet>();
            if (bulletComponent2 != null)
            {
                bulletComponent2.Source = gameObject;
            }
            Rigidbody2D rb2 = bullet2.GetComponent<Rigidbody2D>();
            rb2.AddForce((firePoint2.up * -1) * bulletForce, ForceMode2D.Impulse);

            GameObject bullet3 = Instantiate(bulletPrefab, firePoint3.position, firePoint3.rotation);
            Bullet bulletComponent3 = bullet.GetComponent<Bullet>();
            if (bulletComponent3 != null)
            {
                bulletComponent3.Source = gameObject;
            }
            Rigidbody2D rb3 = bullet3.GetComponent<Rigidbody2D>();
            rb3.AddForce((firePoint3.up * -1) * bulletForce, ForceMode2D.Impulse);

            magStatus -= 1;
        
        
        }
        /*
        if (magStatus > 0)
        {
             for (int i = 0; i <= 2; i++)
             {
                 GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

                 Bullet bulletComponent = bullet.GetComponent<Bullet>();
                 if (bulletComponent != null)
                 {
                     bulletComponent.Source = gameObject;
                 }

                 Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                 //rb.AddForce((firePoint.up * -1) * bulletForce, ForceMode2D.Impulse);

                 switch (i)
                 {
                     case 0:
                         rb.AddForce(firePoint.up * bulletForce + new Vector3(-90f, 0f, 0f));
                         break;
                     case 1:
                         rb.AddForce(firePoint.up * bulletForce + new Vector3(0f, 0f, 0f));
                         break;
                     case 2:
                         rb.AddForce(firePoint.up * bulletForce + new Vector3(90f, 0f, 0f));
                         break;
                 }
                 magStatus -= 1;
             }

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
            float spreadAngle = Random.Range(-10, 10);
            float xPos = firePoint.position.x - player.transform.position.x;
            float yPos = firePoint.position.y - player.transform.position.y;
            float rotateAngle = spreadAngle + (Mathf.Atan(yPos, xPos) * Mathf.Rad2Deg);
            var MovementDirection = new Vector2(Mathf.Cos(rotateAngle * Mathf.Deg2Rad), Mathf.Sin(rotateAngle * Mathf.Deg2Rad)).normalized;
            bulletRB.velocity = MovementDirection * bulletSpeed;
            Destroy(bullet, 5.0f);

            //-------
            //Spawn Round
            var tempBullet = (GameObject)Instantiate(bulletForce, firePoint.position, firePoint.rotation);
            //Get rigidbody
            Rigidbody2D tempBulletRB = tempBullet.GetComponent<RigidBody2D>();
            //Randomize Angle Variation
            float spreadAngle = Random.Range(-10, 10);
            //Take random angle and add it to direction player is aiming
            var x = firePoint.position.x - player.transform.position.x;
            var y = firePoint.position.y - player.transform.position.y;
            float rotateAngle = spreadAngle + (Mathf.Atan(y, x) * Mathf.Rad2Deg);
            //Calculate new direction 
            var MovementDirection = new Vector2(Mathf.Cos(rotateAngle * Mathf.Deg2Rad), Mathf.Sin(rotateAngle * Mathf.Deg2Rad)).normalized;
           
            tempBulletRB.velocity = MovementDirection * bulletSpeed;
            Destroy(tempBullet, 5.0f);
            */
        
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
}
