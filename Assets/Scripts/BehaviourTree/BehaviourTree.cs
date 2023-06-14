using System.Linq;
using UnityEngine;

namespace BehaviourTree {
    public class BehaviourTree : BTNode {
        public BehaviourTree() : base("Tree") { }
        public BehaviourTree(string name) : base(name) { }
        
        public void DebugTree() {
            DebugTree(this);
        }

        public override NodeState Evaluate() {
            return Children[CurrentChild].Evaluate();
        }

        private void DebugTree(BTNode node, int level = 0) {
            var ident = string.Concat(Enumerable.Repeat("-", level));
            Debug.Log($"{ident}N: {node.Name} S: {node.State}");
            
            foreach (var child in node.Children) {
                DebugTree(child, level+1);
            }
        }
    }
}