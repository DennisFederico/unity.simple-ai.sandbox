using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace HospitalSimulation.Goap {
    
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class GoapAction : MonoBehaviour {
        [SerializeField]
        private string _name;
        public string Name => _name;

        public Dictionary<string, int> AfterEffects {
            get => _afterEffects;
            set => _afterEffects = value;
        }

        [SerializeField] public float cost = 1f;
        [SerializeField] public GameObject target;
        [SerializeField] public string locationTargetTag;
        [SerializeField] public float duration = 0f;
        [SerializeField] private WorldState[] preConditions;
        [SerializeField] private WorldState[] afterEffects;
        [SerializeField] public NavMeshAgent agent;
        [SerializeField] protected GoapInventory Inventory;
        [SerializeField] protected WorldStates Beliefs;
        
        public Dictionary<string, int> Preconditions {
            get => _preconditions;
            set => _preconditions = value;
        }


        public WorldStates agentBeliefs;

        public bool running = false;
        
        private Dictionary<string, int> _afterEffects = new ();
        private Dictionary<string, int> _preconditions = new();


        protected GoapAction(string actionName) {
            _name = actionName;
        }
        
        private void Awake() {
            agent = GetComponent<NavMeshAgent>();

            Preconditions = new();
            if (preConditions != null) {
                foreach (var preCondition in preConditions) {
                    Preconditions.Add(preCondition.key, preCondition.value);
                }
            }

            AfterEffects = new();
            if (afterEffects != null) {
                foreach (var afterEffect in afterEffects) {
                    AfterEffects.Add(afterEffect.key, afterEffect.value);
                }
            }
            
            if (target == null && locationTargetTag != "") {
                target = GameObject.FindGameObjectWithTag(locationTargetTag);
            }

            Inventory = GetComponent<GoapAgent>().Inventory;
            Beliefs = GetComponent<GoapAgent>().Beliefs;
        }
        
        public bool IsAchievable() {
            return true;
        }
        
        public bool IsAchievableGiven(Dictionary<string, int> conditions) {
            foreach (var precondition in Preconditions) {
                if (!conditions.ContainsKey(precondition.Key)) {
                    return false;
                }
            }
            return true;
        }
        
        public abstract bool PrePerform();
        public abstract bool PostPerform();
    }
}