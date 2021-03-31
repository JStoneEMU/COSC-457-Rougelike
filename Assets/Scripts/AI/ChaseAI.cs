using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseAI : MonoBehaviour
{
    public GameObject target;
    public float moveSpeed = 5f;
    public float secondsBetweenAI = 1f;

    private List<Vector2Int> path;
    private Vector2 nextPoint;
    private Vector2 colliderSize;
    private Rigidbody2D rb;

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
        }
    }

    void Search()
    {
        PositionSearchProblem problem = new PositionSearchProblem(transform.position, target.transform.position, colliderSize, 1, transform.eulerAngles.z);
        path = AStarSearch<Vector2Int, Vector2Int>.AStar(problem);
        GetNextPoint(transform.position);

        Invoke("Search", secondsBetweenAI);
    }
}