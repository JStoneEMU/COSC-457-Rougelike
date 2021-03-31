using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightlineSearchProblem : SearchProblem<Vector2Int, Vector2Int>
{
    private Vector2Int start;
    private Vector2Int goal;
    private Vector2 colliderSize;
    private float angle;
    private int moveSpeed;

    private float sightDistance;
    private Vector2 target;

    public SightlineSearchProblem(Vector2 s, Vector2 target, Vector2 cSize, int speed, float a, float sight)
    {
        start = Vector2Int.RoundToInt(s);
        this.target = target;
        colliderSize = cSize;
        moveSpeed = speed;
        angle = a;
        sightDistance = sight;       
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
        return Vector2.Distance(state, target) - sightDistance;
    }
    public bool IsGoalState(Vector2Int state)
    {   
        // Check if target in range
        if (Vector2.Distance(state, target) <= sightDistance)
        {
            int layerMask = (1 << 6); // BlockingLayer (walls etc.)
            Vector2 relativePos = target - state;
            // Check if there's a wall in the way
            RaycastHit2D hit = Physics2D.Raycast(state, relativePos, sightDistance, layerMask);
            // Return true if there's no walls in the way
            return (hit.collider == null);
        }
        return false;
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
                    continue;
                }
                successors.Add((nextState, nextAction));
            }
        }
        return successors;
    }
}