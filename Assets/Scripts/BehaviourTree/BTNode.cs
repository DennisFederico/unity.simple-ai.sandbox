using System.Collections.Generic;

namespace BehaviourTree {
    public class BTNode {
        
        public enum NodeState {
            RUNNING,
            SUCCESS,
            FAILURE,
        }
        
        public string Name;
        public NodeState State;
        public List<BTNode> Children = new ();
        protected int CurrentChild = 0;
        
        public BTNode() { }
        
        public BTNode(string name) {
            Name = name;
        }
        
        public BTNode AddChild(BTNode node) {
            Children.Add(node);
            return this;
        }

        public bool IsLeaf() {
            return Children.Count == 0;
        }
        
        public virtual NodeState Evaluate() {
            return Children[CurrentChild].Evaluate();
        }
    }
}