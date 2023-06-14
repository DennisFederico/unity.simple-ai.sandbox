using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HospitalSimulation.Goap {
    public class PlanNode {
        public PlanNode Parent;
        public float RunningCost;
        public Dictionary<string, int> States;
        public GoapAction Action;

        public PlanNode(PlanNode parent, float runningCost, Dictionary<string, int> worldStates, GoapAction action) {
            Parent = parent;
            RunningCost = runningCost;
            States = new Dictionary<string, int>(worldStates);
            Action = action;
        }
        
        public PlanNode(PlanNode parent, float runningCost, Dictionary<string, int> worldStates, Dictionary<string, int> beliefsStates, GoapAction action) {
            Parent = parent;
            RunningCost = runningCost;
            States = new Dictionary<string, int>(worldStates);
            foreach (var belief in beliefsStates) {
                States.TryAdd(belief.Key, belief.Value);
            }
            Action = action;
        }
    }

    public class GoapPlanner {
        public Queue<GoapAction> Plan(List<GoapAction> actions, Dictionary<string, int> goals, WorldStates beliefStates) {
            List<GoapAction> usableActions = new List<GoapAction>();
            foreach (var action in actions) {
                if (action.IsAchievable()) {
                    usableActions.Add(action);
                }
            }

            List<PlanNode> leaves = new List<PlanNode>();
            PlanNode start = new PlanNode(null, 0, World.Instance.States.States, beliefStates.States, null);

            bool success = BuildGraph(start, leaves, usableActions, goals);
            if (!success) {
                return null;
            }

            PlanNode cheapest = null;
            foreach (var leaf in leaves) {
                if (cheapest == null || cheapest.RunningCost > leaf.RunningCost) {
                    cheapest = leaf;    
                }
            }

            PlanNode node = cheapest;
            Stack<GoapAction> actionStack = new Stack<GoapAction>();
            while (node != null) {
                if (node.Action != null) {
                    actionStack.Push(node.Action);
                }

                node = node.Parent;
            }

            Queue<GoapAction> queue = new Queue<GoapAction>(actionStack.ToArray());

            // Debug.Log("The plan is:");
            // foreach (var action in queue) {
            //     Debug.Log($"Q: {action.Name}");
            // }

            return queue;
        }

        private bool BuildGraph(PlanNode parentNode, List<PlanNode> leaves, List<GoapAction> usableActions, Dictionary<string, int> goals) {
            bool foundPath = false;
            foreach (var action in usableActions) {
                if (action.IsAchievableGiven(parentNode.States)) {
                    Dictionary<string, int> currentState = new Dictionary<string, int>(parentNode.States);
                    foreach (var effect in action.AfterEffects) {
                        currentState.TryAdd(effect.Key, effect.Value);
                    }
                    
                    PlanNode node = new PlanNode(parentNode, parentNode.RunningCost + action.cost, currentState, action);
                    if (GoalsAchieved(goals, currentState)) {
                        leaves.Add(node);
                        foundPath = true;
                    } else {
                        foundPath = BuildGraph(node, leaves, ActionSubset(usableActions, action), goals);
                    }
                }
            }

            return foundPath;
        }
        
        private bool GoalsAchieved(Dictionary<string, int> goals, Dictionary<string, int> state) {
            foreach (var goal in goals) {
                if (!state.ContainsKey(goal.Key)) {
                    return false;
                }
            }

            return true;
        }
        
        private List<GoapAction> ActionSubset(List<GoapAction> actions, GoapAction ignoreMe) {
            return actions.Where(action => !action.Equals(ignoreMe)).ToList();
        }
    }
}