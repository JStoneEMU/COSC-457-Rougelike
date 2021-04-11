using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionSearchProblem : SearchProblem<Vector2Int, Vector2Int>
{
    private Vector2Int start;
    private Vector2 goal;
    private Vector2 colliderSize;
    private float angle;
    private int moveSpeed;
    private float followDistance;

    public PositionSearchProblem(Vector2 s, Vector2 g, Vector2 cSize, int speed, float a, float follow)
    {
        start = Vector2Int.RoundToInt(s);
        goal = g;
        colliderSize = cSize;
        moveSpeed = speed;
        angle = a;
        followDistance = follow;
    }

    public Vector2Int GetStartState()
    {
        return start;
    }
    public float GetCost(List<Vector2Int> actions)
    {
        float totalDistance = 0;
        foreach (Vector2Int action in actions)
        {
            totalDistance += Vector2Int.Distance(Vector2Int.zero, action);
        }
        return totalDistance;
    }
    public float Heuristic(Vector2Int state)
    {
        return Vector2.Distance(state, goal) - followDistance;
    }
    public bool IsGoalState(Vector2Int state)
    {
        return (Vector2.Distance(state, goal) <= followDistance);
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
                Collider2D hit = Physics2D.OverlapBox(nextState, colliderSize, angle, layerMask);
                if (hit != null)
                {
                    Debug.Log("checked collider size: " + colliderSize);
                    continue;
                }
                successors.Add((nextState, nextAction));
            }
        }
        return successors;
    }
}