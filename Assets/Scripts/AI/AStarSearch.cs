using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SearchProblem<State, Action>
{
    public State GetStartState();
    public float GetCost(List<Action> actions);
    public float Heuristic(State state);
    public bool IsGoalState(State state);
    public List<(State, Action)> GetSuccessors(State state);
}

public class AStarSearch<State, Action>
{
    static public List<Action> AStar(SearchProblem<State, Action> problem)
    {
        int iterationNum = 0;

        Priority_Queue.SimplePriorityQueue<(State, List<Action>)> fringe = new Priority_Queue.SimplePriorityQueue<(State, List<Action>)>();
        HashSet<State> closedSet = new HashSet<State>();

        fringe.Enqueue((problem.GetStartState(), new List<Action>()), 0);

        while (fringe.Count > 0)
        {
            (State state, List<Action> actions) next = fringe.Dequeue();

            if (problem.IsGoalState(next.state))
                return next.actions;

            if (!closedSet.Contains(next.state))
            {
                closedSet.Add(next.state);
                List<(State, Action)> successors = problem.GetSuccessors(next.state);

                if (++iterationNum > 1000)
                {
                    Debug.Log("A* is iterating too much");
                    break;
                }

                foreach ((State state, Action action) successor in successors)
                {
                    List<Action> newActions = new List<Action>(next.actions);
                    newActions.Add(successor.action);
                    float priority = problem.GetCost(newActions) + problem.Heuristic(successor.state);
                    fringe.Enqueue((successor.state, newActions), priority);
                }
            }
        }

        return new List<Action>();
    }
}
