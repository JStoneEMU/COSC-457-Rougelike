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
    private AudioSource pistol;
    private AudioSource reload;

    void Start()
    {
        AudioSource[] aSources = GetComponents<AudioSource>();
        pistol = aSources[0];
        reload = aSources[1];
    }

    // Update is called once per frame
    void Update()
    {
        if (takeInput)
        {
            magDisplay.text = magStatus + "/17";
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
                pistol.Play();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (magStatus > 0) //Load 1 in chamber plus full mag
                {
                    magStatus = 18;
                    reload.Play();
                }
                if (magStatus == 0) //Load full mag
                {
                    magStatus = 17;
                    reload.Play();
                }
                if (magStatus == 18)//Swap full mag for another full mag
                {
                    //Do nothing
                    reload.Play();
                }
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
