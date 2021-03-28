using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minimax
{
    public class Agent
    {
        public Vector2 Position { get; set; }
        public int Health { get; set; }

        public float AttackRange { get; set; }
        public int AttackDamage { get; set; }
        public float MoveSpeed { get; set; }
        public Vector2 ColliderSize { get; set; }

        public Agent(Vector2 position, int health, int range, int damage, float moveSpeed, Vector2 colliderSize)
        {
            Position = position;
            Health = health;
            AttackRange = range;
            AttackDamage = damage;
            MoveSpeed = moveSpeed;
            ColliderSize = colliderSize;
        }

        public Agent(Agent other)
        {
            Position = other.Position;
            Health = other.Health;
            AttackRange = other.AttackRange;
            AttackDamage = other.AttackDamage;
            MoveSpeed = other.MoveSpeed;
            ColliderSize = other.ColliderSize;
        }
    }

    public class Action
    {
        public enum Type
        {
            Move,
            Attack
        }

        public Type ActionType { get; set; } = Type.Move;
        public Vector2 Position { get; set; } = Vector2.zero;
        public int TargetIndex { get; set; } = -1;

        public Action(Vector2 pos, Type type)
        {
            Position = pos;
            ActionType = type;
        }

        public Action(Vector2 pos, Type type, int targetIndex)
        {
            Position = pos;
            ActionType = type;
            TargetIndex = targetIndex;
        }
    }

    public class State
    {
        private List<Agent> agentList;
        private int playerIndex;
        private float timeBetweenStates = 0.2f;

        public State(List<Agent> agentList, int playerIndex, float timeBetweenStates)
        {
            this.agentList = agentList;
            this.playerIndex = playerIndex;
            this.timeBetweenStates = timeBetweenStates;
        }

        // All non-player agents are dead
        public bool IsLose()
        {
            for (int i = 0; i < agentList.Count; i++)
            {
                if (i != playerIndex && agentList[i].Health > 0)
                {
                    return false;
                }
            }
            return true;
        }

        // Player is dead
        public bool IsWin()
        {
            return agentList[playerIndex].Health <= 0;
        }

        public List<Action> GetLegalActions(int agentIndex)
        {
            List<Action> legalActions = new List<Action>();
            Agent agent = agentList[agentIndex];

            // Player can attack non-players
            if (agentIndex == playerIndex)
            {
                for (int i = 0; i < agentList.Count; i++)
                {
                    if (i != playerIndex)
                    {
                        Vector2 enemyPos = agentList[i].Position;
                        float distance = Vector2.Distance(agent.Position, enemyPos);
                        if (distance <= agent.AttackRange)
                        {
                            int layerMask = 1 << 6; // BlockingLayer (walls etc.)
                            Vector2 relativeAttackPos = enemyPos - agent.Position;
                            // Check if there's a wall in the way
                            RaycastHit2D hit = Physics2D.Raycast(agent.Position, relativeAttackPos, distance, layerMask);
                            if (hit.collider == null)   // Nothing in the way
                            {
                                Action attackAction = new Action(relativeAttackPos, Action.Type.Attack, i);
                                legalActions.Add(attackAction);
                            }
                        }
                    }
                }
            }
            // Non-players can attack player
            else
            {
                Vector2 playerPos = agentList[playerIndex].Position;
                float distance = Vector2.Distance(agent.Position, playerPos);
                if (distance <= agent.AttackRange)
                {
                    int layerMask = 1 << 6; // BlockingLayer (walls etc.)
                    Vector2 relativeAttackPos = playerPos - agent.Position;
                    // Check if there's a wall in the way
                    RaycastHit2D hit = Physics2D.Raycast(agent.Position, relativeAttackPos, distance, layerMask);
                    if (hit.collider == null)   // Nothing in the way
                    {
                        Action attackAction = new Action(relativeAttackPos, Action.Type.Attack, playerIndex);
                        legalActions.Add(attackAction);
                    }
                }
            }

            // Check all 8 directions
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Vector2 move = new Vector2(x, y);
                    // Make vector length = 1 even if its diagonal
                    move.Normalize();
                    move = move * agent.MoveSpeed * timeBetweenStates;

                    // Check for collision
                    Vector2 nextPos = agent.Position + move;
                    int layerMask = 1 << 6; // BlockingLayer (walls etc.)
                    Collider2D hit = Physics2D.OverlapBox(nextPos, agent.ColliderSize, 0, layerMask);
                    if (hit == null)    // No collision
                    {
                        Action moveAction = new Action(move, Action.Type.Move);
                        legalActions.Add(moveAction);
                    }
                }
            }

            return legalActions;
        }

        public State GenerateSuccessor(int agentIndex, Action action)
        {
            Agent agent = new Agent(agentList[agentIndex]);
            List<Agent> newAgentList = new List<Agent>(agentList);
            newAgentList[agentIndex] = agent;

            if (action.ActionType == Action.Type.Attack)
            {
                Agent target = new Agent(agentList[action.TargetIndex]);
                newAgentList[action.TargetIndex] = target;
                target.Health -= agent.AttackDamage;
            }
            else if (action.ActionType == Action.Type.Move)
            {
                agent.Position += action.Position;
            }

            return new State(newAgentList, playerIndex, timeBetweenStates);
        }

        public int GetNumAgents()
        {
            return agentList.Count;
        }

        public int GetPlayerIndex()
        {
            return playerIndex;
        }

        public List<Agent> GetEnemies()
        {
            List<Agent> enemyList = new List<Agent>(agentList);
            enemyList.RemoveAt(playerIndex);
            return enemyList;
        }

        public Agent GetPlayer()
        {
            return agentList[playerIndex];
        }
    }
}
