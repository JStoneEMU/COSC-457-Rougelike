using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Minimax;

public class MinimaxSearchAI : MonoBehaviour
{
    public GameObject player;
    public int maxDepth = 1;
    public float secondsBetweenAI = .2f;

    private Action[] nextActions;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("MinimaxSearch", 1);
    }

    void MinimaxSearch()
    {
        List<Agent> agentList = new List<Agent>();
        List<GameObject> agentObjectList = new List<GameObject>();

        GameObject[] smartEnemyArr = GameObject.FindGameObjectsWithTag("SmartEnemy");

        foreach (GameObject smartEnemy in smartEnemyArr)
        {
            agentObjectList.Add(smartEnemy);

            Enemy enemyComponent = smartEnemy.GetComponent<Enemy>();
            MinimaxEnemy minimaxComponent = smartEnemy.GetComponent<MinimaxEnemy>();

            Vector2 position = smartEnemy.transform.position;
            int health = (int) enemyComponent.CurrentHealth;
            int range = enemyComponent.attackRange;
            int damage = enemyComponent.attackDamage;
            float moveSpeed = enemyComponent.moveSpeed;
            BoxCollider2D collider = smartEnemy.GetComponent<BoxCollider2D>();
            Vector2 scale = smartEnemy.transform.localScale;
            Vector2 colliderSize = collider.size * scale;

            Agent newAgent = new Agent(position, health, range, damage, moveSpeed, colliderSize);
            newAgent.ShotTimer = minimaxComponent.ShotTimer;
            newAgent.ShotCooldown = minimaxComponent.shotCooldown;
            agentList.Add(newAgent);
        }

        agentObjectList.Add(player);

        // TEMPORARY HARD-CODED VALUES
        agentList.Add(new Agent(player.transform.position, 50, 10, 10, 5, new Vector2(1.323f, 1.323f)));

        int playerIndex = agentList.Count - 1;
        nextActions = new Action[agentList.Count];
        State gameState = new State(agentList, playerIndex, secondsBetweenAI);

        (float value, Action action) thisV = Value(gameState, 0, 0);

        // Give actions to smart enemies
        for (int i = 0; i < playerIndex; i++)
        {
            MinimaxEnemy enemy = smartEnemyArr[i].GetComponent<MinimaxEnemy>();
            enemy.Player = player;
            if (enemy != null && nextActions[i] != null)
            {              
                if (nextActions[i].ActionType == Action.Type.Move)
                {
                    enemy.Move(nextActions[i].Position);
                }
                else if (nextActions[i].ActionType == Action.Type.Attack)
                {
                    enemy.Attack(nextActions[i].Position);
                }
            }
        }

        Invoke("MinimaxSearch", secondsBetweenAI);
    }

    (float, Action) Value(State gameState, int agentIndex, int depth)
    {
        if (gameState.IsLose() || gameState.IsWin() || depth == maxDepth)
        {
            return (EvaluationFunction(gameState), null);
        }
        if (agentIndex == gameState.GetPlayerIndex())
        {
            return MinValue(gameState, agentIndex, depth);
        }
        else    // Enemy
        {
            (float value, Action action) max = MaxValue(gameState, agentIndex, depth);
            nextActions[agentIndex] = max.action;
            return max;
        }
    }

    private (float, Action) MaxValue(State gameState, int agentIndex, int depth)
    {
        int nextAgentIndex = agentIndex + 1;
        if (gameState.GetNumAgents() == nextAgentIndex)
        {
            nextAgentIndex = 0;
            depth = depth + 1;
        }

        float bestValue = float.NegativeInfinity;
        Action bestAction = null;
        foreach (Action action in gameState.GetLegalActions(agentIndex))
        {
            State nextGameState = gameState.GenerateSuccessor(agentIndex, action);
            (float value, Action nextAction) next = Value(nextGameState, nextAgentIndex, depth);
            if (bestValue < next.value)
            {
                bestValue = next.value;
                bestAction = action;
            }
        }
        return (bestValue, bestAction);
    }

    private (float, Action) MinValue(State gameState, int agentIndex, int depth)
    {
        int nextAgentIndex = agentIndex + 1;
        if (gameState.GetNumAgents() == nextAgentIndex)
        {
            nextAgentIndex = 0;
            depth = depth + 1;
        }

        float bestValue = float.PositiveInfinity;
        Action bestAction = null;
        foreach(Action action in gameState.GetLegalActions(agentIndex))
        {
            State nextGameState = gameState.GenerateSuccessor(agentIndex, action);
            (float value, Action nextAction) next = Value(nextGameState, nextAgentIndex, depth);
            if (bestValue > next.value)
            {
                bestValue = next.value;
                bestAction = action;
            }
        }
        return (bestValue, bestAction);
    }

    private float EvaluationFunction(State gameState)
    {
        Agent player = gameState.GetPlayer();
        List<Agent> enemies = gameState.GetEnemies();

        float sumDistance = 0;
        float spread = 0;
        int sumHealth = 0;
        float moveBonus = 0;

        foreach (Agent enemy in enemies)
        {
            // try to be exactly at max range from player
            float distance = Vector2.Distance(enemy.Position, player.Position);
            sumDistance += Mathf.Abs((enemy.AttackRange - 0.5f) - distance);

            sumHealth += enemy.Health;

            float minSpread = float.PositiveInfinity;
            foreach (Agent other in enemies)
            {
                if (enemy != other)
                {
                    float dist = Vector2.Distance(enemy.Position, other.Position);
                    minSpread = Mathf.Min(minSpread, dist);
                }
            }

            if (minSpread != float.PositiveInfinity)
                spread += minSpread;

            // Can't shoot because it's on cooldown, so prefer states with movement
            if (enemy.ShotTimer > 0)
            {
                if (Vector2.Distance(enemy.Position, enemy.PrevPosition) < 0.5)
                {
                    moveBonus -= 100;
                }
                else
                {
                    moveBonus += 100;
                }
            }
            else
            {
                moveBonus -= 200;
            }
        }

        return (sumHealth / 10) - (player.Health * 10) - (sumDistance * 4) + (spread / 2) + moveBonus;
    }
}
