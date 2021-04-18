using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimaxEnemy : MonoBehaviour
{
    public float shotCooldown = 2;

    public GameObject Player { get; set; }

    private Enemy enemyComponent;
    private Vector2 nextPoint;
    private State currentState = State.None;
    private SeekAI seekComponent;
    private Enemy shootingComponent;

    public float ShotTimer { get; private set; } = 0;

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
        enemyComponent = GetComponent<Enemy>();
        nextPoint = transform.position;
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

        if (ShotTimer > 0)
        {
            ShotTimer -= Time.deltaTime;
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
                enemyComponent.MoveTowards(nextPoint, Time.deltaTime);
            }
            else
            {
                currentState = State.None;
                enemyComponent.LookingAt = Player;
            }
        }
    }

    public void Move(Vector2 relativePos)
    {
        Vector2 position = transform.position;
        nextPoint = position + relativePos;
        currentState = State.Move;
        enemyComponent.LookingAt = null;
    }

    public void Attack(Vector2 relativePos)
    {
        if (shootingComponent != null)
        {
            if (ShotTimer <= 0)
            {
                shootingComponent.ShootAt(relativePos);
                ShotTimer = shotCooldown;
            }
        }
    }

}
