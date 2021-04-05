using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shooting : MonoBehaviour
{
    public bool takeInput = false;
    public Transform firePoint;
    public GameObject bulletPrefab;

    public TextMeshProUGUI magDisplay;
    public int magStatus = 17;

    public float bulletForce = 20f;

    // Update is called once per frame
    void Update()
    {
        magDisplay.text = magStatus + "/17";
        if (takeInput && Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (magStatus > 0) //Load 1 in chamber plus full mag
            {
                magStatus = 18;

            }
            if (magStatus == 0) //Load full mag
            {
                magStatus = 17;
            }
            if (magStatus == 18)//Swap full mag for another full mag
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
             magStatus -= 1;
        }
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
