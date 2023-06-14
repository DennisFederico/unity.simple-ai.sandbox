using System.Collections.Generic;
using UnityEngine;

namespace Graph {
    public class Node {
        public List<Edge> OutboundEdges = new();
        // public Node Path = null;
        // private Vector3 _position;
        public float F, G, H;
        public Node From;

        public Node(GameObject id) {
            this.Id = id;
            // _position = id.transform.position;
        }
        
        public GameObject Id { get; }
    }
}