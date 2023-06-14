namespace Graph {
    public class Edge {
        public Node StartNode { get; }
        public Node EndNode { get; }

        public Edge(Node startNode, Node endNode) {
            StartNode = startNode;
            EndNode = endNode;
        }
        
        
        public override string ToString() {
            return $"Edge: {StartNode} -> {EndNode}";
        }
    }
}