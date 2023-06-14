namespace BehaviourTree {
    public class BTSequenceNode : BTNode {
        public BTSequenceNode(string name) : base(name) { }

        public override NodeState Evaluate() {
            var childState = Children[CurrentChild].Evaluate();
            
            switch (childState) {
                case NodeState.RUNNING:
                    return NodeState.RUNNING;
                case NodeState.FAILURE:
                    CurrentChild = 0;
                    return NodeState.FAILURE;
                case NodeState.SUCCESS:
                    CurrentChild++;
                    if (CurrentChild < Children.Count) return NodeState.RUNNING;
                    CurrentChild = 0;
                    return NodeState.SUCCESS;
                default:
                    return NodeState.RUNNING;
            }
        }
    }
}