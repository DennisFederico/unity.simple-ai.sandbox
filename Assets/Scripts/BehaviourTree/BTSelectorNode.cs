namespace BehaviourTree {
    public class BTSelectorNode : BTNode {
        public BTSelectorNode(string name) : base(name) { }

        public override NodeState Evaluate() {
            var childState = Children[CurrentChild].Evaluate();
            
            switch (childState) {
                case NodeState.RUNNING:
                    return NodeState.RUNNING;
                case NodeState.SUCCESS:
                    CurrentChild = 0;
                    return NodeState.SUCCESS;
                case NodeState.FAILURE:
                    CurrentChild++;
                    if (CurrentChild < Children.Count) return NodeState.RUNNING;
                    CurrentChild = 0;
                    return NodeState.FAILURE;
                default:
                    return NodeState.RUNNING;
            }
        }
    }
}