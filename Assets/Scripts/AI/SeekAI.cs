using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekAI : MonoBehaviour
{
    public GameObject target;
    public float sightDistance = 5f;
    public float moveSpeed = 5f;
    public float secondsBetweenAI = 1f;

    public State CurrentState { get; private set; } = State.Idle;

    private List<Vector2Int> path;
    private Vector2 nextPoint;
    private Vector2 colliderSize;
    private Rigidbody2D rb;
    private bool visible = false;

    public enum State
    {
        Idle,
        Seek,
        Found
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        nextPoint = transform.position;
        path = new List<Vector2Int>();
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        colliderSize = collider.size;

        Invoke("Search", 1f);
    }

    void OnBecameVisible()
    {
        visible = true;
    }

    void OnBecameInvisible()
    {
        visible = false;
    }

    void FixedUpdate()
    {
        Vector2 position = transform.position;
        if (position == nextPoint)
        {
            GetNextPoint(position);
        }

        position = Vector2.MoveTowards(position, nextPoint, moveSpeed * Time.deltaTime);
        rb.MovePosition(position);
    }

    void GetNextPoint(Vector2 position)
    {
        if (path.Count > 0)
        {           
            Vector2 nextAction = path[0];
            path.RemoveAt(0);
            nextPoint = position + nextAction;

            if (path.Count == 0)
                CurrentState = State.Found;
        }
    }

    void Search()
    {
        if (visible)
        {
            SightlineSearchProblem problem = new SightlineSearchProblem(transform.position, target.transform.position, colliderSize, 1, transform.eulerAngles.z, sightDistance);
            path = AStarSearch<Vector2Int, Vector2Int>.AStar(problem);

            if (path.Count > 0)
            {
                CurrentState = State.Seek;
            }
            else
            {
                CurrentState = State.Found;
            }

            GetNextPoint(transform.position);
        }

        Invoke("Search", secondsBetweenAI);
    }
}
