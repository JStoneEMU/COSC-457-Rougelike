using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AKShooting : MonoBehaviour
{
    public bool takeInput = false;
    public Transform firePoint;
    public GameObject bulletPrefab;

    public TextMeshProUGUI magDisplay;
    public int magStatus = 30;

    public float bulletForce = 20f;

    bool canShoot = true;

    public float shootInterval = 0.15f;

    public AudioSource pistol;
    public AudioSource reload;
    public AudioSource heal;
    public AudioSource shotgun;
    public AudioSource ak;

    void Start()
    {
        /*AudioSource[] aSources = GetComponents<AudioSource>();
        pistol = aSources[0];
        reload = aSources[1];
        heal = aSources[2];
        shotgun = aSources[3];
        ak = aSources[4];*/
    }


    // Update is called once per frame
    void Update()
    {
        magDisplay.text = magStatus + "/30";
        if (takeInput && Input.GetButton("Fire1") && canShoot == true)
        {
            StartCoroutine(Shoot());
            ak.Play();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            reload.Play();
            
            if (magStatus > 0) //Load 1 in chamber plus full mag
            {
                magStatus = 31;
            }
            if (magStatus == 0) //Load full mag
            {
                magStatus = 30;
            }
            if (magStatus == 31)//Swap full mag for another full mag
            {
                //Do nothing
            }
        }
    }
    IEnumerator Shoot()
    {
        if (magStatus > 0)
        {
            canShoot = false;

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            Bullet bulletComponent = bullet.GetComponent<Bullet>();
            if (bulletComponent != null)
            {
                bulletComponent.Source = gameObject;
            }

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce((firePoint.up * -1) * bulletForce, ForceMode2D.Impulse);

            yield return new WaitForSeconds(shootInterval);
            magStatus -= 1;
            canShoot = true;
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
