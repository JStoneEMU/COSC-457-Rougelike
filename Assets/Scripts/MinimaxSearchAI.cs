using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Minimax;

public class MinimaxSearchAI : MonoBehaviour
{
    public GameObject player;
    public int maxDepth = 2;
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
            agentList.Add(new Agent(smartEnemy.transform.position, 100, 5, 1, 5, new Vector2(1, 1)));
        }

        agentObjectList.Add(player);

        // TEMPORARY HARD-CODED VALUES
        agentList.Add(new Agent(player.transform.position, 100, 5, 5, 5, new Vector2(0.4f, 0.4f)));

        int playerIndex = agentList.Count - 1;
        nextActions = new Action[agentList.Count];
        State gameState = new State(agentList, playerIndex, secondsBetweenAI);
        float initialAlpha = float.NegativeInfinity;
        float initialBeta = float.PositiveInfinity;

        (float value, Action action) thisV = Value(gameState, 0, 0, initialAlpha, initialBeta);
        //print("Returned: " + thisV.action.Position);

        // Give actions to smart enemies
        for (int i = 0; i < playerIndex; i++)
        {
            if (nextActions[i].ActionType == Action.Type.Attack)
            {
                print("Stored: " + nextActions[i].Position);
                print(nextActions[i].ActionType);
            }

            MinimaxEnemy enemy = smartEnemyArr[i].GetComponent<MinimaxEnemy>();
            if (enemy != null && nextActions[i] != null)
            {
                if (nextActions[i].ActionType == Action.Type.Move)
                {

                    enemy.Move(nextActions[i].Position);
                }
            }
        }

        Invoke("MinimaxSearch", secondsBetweenAI);
    }

    (float, Action) Value(State gameState, int agentIndex, int depth, float alpha, float beta)
    {
        if (gameState.IsLose() || gameState.IsWin() || depth == maxDepth)
        {
            return (EvaluationFunction(gameState), null);
        }
        if (agentIndex == gameState.GetPlayerIndex())
        {
            return MinValue(gameState, agentIndex, depth, alpha, beta);
        }
        else    // Enemy
        {
            (float value, Action action) max = MaxValue(gameState, agentIndex, depth, alpha, beta);
            nextActions[agentIndex] = max.action;
            return max;
        }
    }

    private (float, Action) MaxValue(State gameState, int agentIndex, int depth, float alpha, float beta)
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
            (float value, Action nextAction) next = Value(nextGameState, nextAgentIndex, depth, alpha, beta);
            if (bestValue < next.value)
            {
                bestValue = next.value;
                bestAction = action;
            }
            // Alpha beta pruning
            if (bestValue > beta)
            {
                return (bestValue, bestAction);
            }
            alpha = System.Math.Max(alpha, bestValue);
        }
        return (bestValue, bestAction);
    }

    private (float, Action) MinValue(State gameState, int agentIndex, int depth, float alpha, float beta)
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
            (float value, Action nextAction) next = Value(nextGameState, nextAgentIndex, depth, alpha, beta);
            if (bestValue > next.value)
            {
                bestValue = next.value;
                bestAction = action;
            }
            // Alpha beta pruning
            if (bestValue < alpha)
            {
                return (bestValue, bestAction);
            }
            beta = System.Math.Min(beta, bestValue);
        }
        return (bestValue, bestAction);
    }

    private float EvaluationFunction(State gameState)
    {
        Agent player = gameState.GetPlayer();
        List<Agent> enemies = gameState.GetEnemies();

        float sumDistance = 0;
        foreach (Agent enemy in enemies)
        {
            sumDistance += Vector2.Distance(enemy.Position, player.Position);
        }

        return -sumDistance;

        //return -player.Health;
        //return 0;
    }
}
