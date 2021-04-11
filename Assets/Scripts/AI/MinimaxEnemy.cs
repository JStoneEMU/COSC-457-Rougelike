using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimaxEnemy : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float attackRange = 5f;
    public int attackDamage = 1;
    public int maxHealth = 10;
    public float shotCooldown = 2;

    public int CurrentHealth { get; set; }

    private Vector2 nextPoint;
    private Rigidbody2D rb;
    private State currentState = State.None;
    private SeekAI seekComponent;
    private Enemy shootingComponent;
    private float shotTimer = 0;

    enum State
    {
        None,
        Move,
        Attack
    }

    // Start is called before the first frame update
    void Start()
    {
        seekComponent = GetComponent<SeekAI>();
        shootingComponent = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody2D>();
        nextPoint = transform.position;
        CurrentHealth = maxHealth;
    }

    void Update()
    {
        // If found target, and number enemies using Minimax is < 3, use Minimax (by tagging self as "SmartEnemy")
        if (seekComponent != null && seekComponent.enabled)
        {
            if (seekComponent.CurrentState == SeekAI.State.Found)
            {
                if (tag == "Untagged")
                {
                    GameObject[] smartEnemyArr = GameObject.FindGameObjectsWithTag("SmartEnemy");
                    if (smartEnemyArr.Length < 3)
                    {
                        tag = "SmartEnemy";
                        seekComponent.enabled = false;
                    }
                }
            }
        }

        if (shotTimer > 0)
        {
            shotTimer -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        if (currentState == State.Move)
        {
            Vector2 position = transform.position;

            //if (Vector2.Distance(position, nextPoint) > 0.001)
            if (position != nextPoint)
            {
                position = Vector2.MoveTowards(position, nextPoint, moveSpeed * Time.deltaTime);
                rb.MovePosition(position);
            }
            else
            {
                currentState = State.None;
            }
        }
    }

    public void Move(Vector2 relativePos)
    {
        Vector2 position = transform.position;
        nextPoint = position + relativePos;
        currentState = State.Move;
    }

    public void Attack(Vector2 relativePos)
    {
        if (shootingComponent != null)
        {
            if (shotTimer <= 0)
            {
                shootingComponent.ShootAt(relativePos);
                shotTimer = shotCooldown;
            }
        }
    }

}
