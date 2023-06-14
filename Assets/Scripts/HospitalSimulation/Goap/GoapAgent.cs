using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HospitalSimulation.Goap {

    public class SubGoal {
        public Dictionary<string, int> sGoals;
        public bool removeAfterFulfill;
        
        public SubGoal(string key, int value, bool remove) {
            sGoals = new Dictionary<string, int>();
            sGoals.Add(key, value);
            removeAfterFulfill = remove;
        }
    }
    public class GoapAgent : MonoBehaviour {
        [SerializeField] private List<GoapAction> availableActions = new();
        public Dictionary<SubGoal, int> goals = new();
        public readonly GoapInventory Inventory = new ();
        public WorldStates Beliefs = new ();
        
        private GoapPlanner _planner;
        public Queue<GoapAction> ActionsQueue = new ();
        public GoapAction currentAction;
        private SubGoal _currentGoal;
        [SerializeField] private bool debug;
        
         
        protected void Start() {
            GoapAction[] actions = GetComponents<GoapAction>();
            foreach (var action in actions) {
                availableActions.Add(action);
            }
        }

        private bool invoked = false;
        protected void LateUpdate() {
            
            if (currentAction != null && currentAction.running) {
                if (currentAction.agent.hasPath && currentAction.agent.remainingDistance < 1f) {
                    if (!invoked) {
                        Invoke(nameof(CompleteAction), currentAction.duration);
                        invoked = true;
                    }
                }
                return;
            }
            
            if (_planner == null || ActionsQueue == null) {
                _planner = new GoapPlanner();
                //SORT GOALS BY PRIORITY
                goals = SortGoals(goals);
                foreach (var goal in goals) {
                    ActionsQueue = _planner.Plan(availableActions, goal.Key.sGoals, Beliefs);
                    if (ActionsQueue is { Count: > 0 }) {
                        _currentGoal = goal.Key;
                        break;
                    }
                }
            }

            if (ActionsQueue is { Count: 0 }) {
                if (_currentGoal.removeAfterFulfill) {
                    goals.Remove(_currentGoal);
                }

                _planner = null;
            }
            
            
            if (ActionsQueue is { Count: > 0 }) {
                currentAction = ActionsQueue.Dequeue();
                if (currentAction.PrePerform()) {
                    if (currentAction.target == null && currentAction.locationTargetTag != null) {
                        currentAction.target = GameObject.FindGameObjectWithTag(currentAction.locationTargetTag);
                    }

                    if (currentAction.target != null) {
                        currentAction.running = true;
                        currentAction.agent.SetDestination(currentAction.target.transform.position);    
                    }
                } else {
                    ActionsQueue = null;
                }
            }
        }

        void CompleteAction() {
            currentAction.running = false;
            currentAction.PostPerform();
            invoked = false;
        }
        
        private Dictionary<SubGoal, int> SortGoals(Dictionary<SubGoal, int> goals) {
            var sortedGoals = goals.ToList();
            sortedGoals.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
            return sortedGoals.ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }
}