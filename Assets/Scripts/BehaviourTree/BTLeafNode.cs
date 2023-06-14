using UnityEngine;

namespace BehaviourTree {
    public class BTLeafNode : BTNode {
        
        public delegate NodeState Action();
        private readonly Action _action;
        
        public BTLeafNode(string name, Action action) : base(name) {
            _action = action;
        }

        public override NodeState Evaluate() {
            var nodeState = _action?.Invoke() ?? NodeState.FAILURE;
            Debug.Log($"{Name} evaluated to {nodeState}");
            return nodeState; 
        }
    }
}