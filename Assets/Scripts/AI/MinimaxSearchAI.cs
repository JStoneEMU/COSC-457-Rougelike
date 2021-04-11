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

            // TEMPORARY HARD-CODED VALUES
            agentList.Add(new Agent(smartEnemy.transform.position, 100, 10, 1, 5, new Vector2(1, 1)));
        }

        agentObjectList.Add(player);

        // TEMPORARY HARD-CODED VALUES
        agentList.Add(new Agent(player.transform.position, 100, 10, 5, 5, new Vector2(0.4f, 0.4f)));

        int playerIndex = agentList.Count - 1;
        nextActions = new Action[agentList.Count];
        State gameState = new State(agentList, playerIndex, secondsBetweenAI);

        (float value, Action action) thisV = Value(gameState, 0, 0);
        //print("Returned: " + thisV.action.Position);

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
        foreach (Agent enemy in enemies)
        {
            sumDistance += Mathf.Max(enemy.AttackRange, Vector2.Distance(enemy.Position, player.Position));
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
        }

        return (sumHealth / 10) - player.Health - sumDistance + (spread / 2);

        //return -player.Health;
    }
}
