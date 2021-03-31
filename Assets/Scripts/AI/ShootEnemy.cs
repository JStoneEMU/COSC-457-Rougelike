using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Uses SeekAI component to find target, then shoots it

public class ShootEnemy : MonoBehaviour
{
    public float shotCooldown = 2;

    private SeekAI seekComponent;
    private GameObject target;
    private Shooting shootingComponent;
    private float shotTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        seekComponent = GetComponent<SeekAI>();
        shootingComponent = GetComponent<Shooting>();
        if (seekComponent != null)
            target = seekComponent.target;
    }

    void Update()
    {
        if (target != null && seekComponent.CurrentState == SeekAI.State.Found)
        {
            Vector2 pos = transform.position;
            Vector2 targetPos = target.transform.position;
            Vector2 attackAngle = targetPos - pos;
            //print("Shooting at: " + attackAngle);
            if (shootingComponent != null)
            {
                if (shotTimer <= 0)
                {
                    shootingComponent.ShootAt(attackAngle);
                    shotTimer = shotCooldown;
                }
                else
                {
                    shotTimer -= Time.deltaTime;
                }
            }
        }
    }

}
