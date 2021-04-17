using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekAI : MonoBehaviour
{
    public GameObject target;
    public float sightDistance = 5f;
    public float secondsBetweenAI = 1f;

    public State CurrentState { get; private set; } = State.Idle;

    private Enemy enemyComponent;
    private List<Vector2Int> path;
    private Vector2 nextPoint;
    private Vector2 colliderSize;
    private bool visible = false;
    public AudioSource flesh;

    public enum State
    {
        Idle,
        Seek,
        Found
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyComponent = GetComponent<Enemy>();
        nextPoint = transform.position;
        path = new List<Vector2Int>();
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        Vector2 scale = transform.localScale;
        colliderSize = collider.size * scale;

        Invoke("Search", 0f);
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

        if (position != nextPoint)
            enemyComponent.MoveTowards(nextPoint, Time.deltaTime);
    }

    void GetNextPoint(Vector2 position)
    {
        if (path.Count > 0)
        {           
            Vector2 nextAction = path[0];
            path.RemoveAt(0);
            nextPoint = position + nextAction;

            if (path.Count == 0)
                FoundState();
        }
    }

    void Search()
    {
        if (visible)
        {
            SightlineSearchProblem problem = new SightlineSearchProblem(transform.position, target.transform.position, colliderSize, 1, 0, sightDistance);
            path = AStarSearch<Vector2Int, Vector2Int>.AStar(problem);
            flesh.Play();
            
            if (path.Count > 0)
            {
                SeekState();
            }
            else
            {
                FoundState();
            }

            GetNextPoint(transform.position);
            
        }

        Invoke("Search", secondsBetweenAI);
    }

    void FoundState()
    {
        CurrentState = State.Found;
        enemyComponent.LookingAt = target;
    }

    void SeekState()
    {
        CurrentState = State.Seek;
        enemyComponent.LookingAt = null;
    }
}
