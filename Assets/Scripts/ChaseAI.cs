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
        if (position == nextPoint && path.Count > 0)
        {
            Vector2 nextAction = path[0];
            path.RemoveAt(0);
            nextPoint = position + nextAction;
        }

        position = Vector2.MoveTowards(position, nextPoint, moveSpeed * Time.deltaTime);
        rb.MovePosition(position);
    }

    void Search()
    {
        PositionSearchProblem problem = new PositionSearchProblem(transform.position, target.transform.position, colliderSize, 1);
        path = AStarSearch<Vector2Int, Vector2Int>.AStar(problem);

        Invoke("Search", secondsBetweenAI);
    }
}

public class PositionSearchProblem : SearchProblem<Vector2Int, Vector2Int>
{
    private Vector2Int start;
    private Vector2Int goal;
    private Vector2 colliderSize;
    private int moveSpeed;

    public PositionSearchProblem(Vector2 s, Vector2 g, Vector2 cSize, int speed)
    {
        start = Vector2Int.RoundToInt(s);
        goal = Vector2Int.RoundToInt(g);
        colliderSize = cSize;
        moveSpeed = speed;
    }

    public Vector2Int GetStartState()
    {
        return start;
    }
    public float GetCost(List<Vector2Int> actions)
    {
        return actions.Count * moveSpeed;
    }
    public float Heuristic(Vector2Int state)
    {
        return Vector2.Distance(state, goal);
    }
    public bool IsGoalState(Vector2Int state)
    {
        return (Vector2.Distance(state, goal) <= colliderSize.x);
    }
    public List<(Vector2Int, Vector2Int)> GetSuccessors(Vector2Int state)
    {
        List<(Vector2Int, Vector2Int)> successors = new List<(Vector2Int, Vector2Int)>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2Int nextAction = new Vector2Int(x, y);
                nextAction = nextAction * moveSpeed;
                Vector2Int nextState = state + nextAction;

                int layerMask = 1 << 6;
                Collider2D hit = Physics2D.OverlapBox(nextState, colliderSize, 0, layerMask);
                if (hit != null)
                {
                    continue;
                }
                successors.Add((nextState, nextAction));
            }
        }
        return successors;
    }
}
