using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Minimax;

public class MinimaxSearchAI : MonoBehaviour
{
    public int maxDepth = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
            return MaxValue(gameState, agentIndex, depth, alpha, beta);
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
        return 0;
    }
}
