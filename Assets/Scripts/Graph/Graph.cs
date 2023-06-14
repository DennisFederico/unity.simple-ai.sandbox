using System.Collections.Generic;
using UnityEngine;

namespace Graph {
    public class Graph {
        List<Edge> _edges = new();
        List<Node> _nodes = new();
        public List<Node> Paths { get; } = new();

        public Graph() {}

        public void AddNode(GameObject id) {
            _nodes.Add(new Node(id));
        }
        
        public void AddEdge(GameObject startId, GameObject endId) {
            Node startNode = _nodes.Find(node => node.Id == startId);
            Node endNode = _nodes.Find(node => node.Id == endId);

            if (startNode != null && endNode != null) {
                var edge = new Edge(startNode, endNode);
                _edges.Add(edge);
                startNode.OutboundEdges.Add(edge);
            }
        }

        public bool AStar(GameObject startId, GameObject endId) {
            
            if (startId == endId) {
                Paths.Clear();
                return false;
            }
            
            Node startNode = _nodes.Find(node => node.Id == startId);
            Node endNode = _nodes.Find(node => node.Id == endId);

            if (startNode != null && endNode != null) {
                List<Node> openList = new();
                List<Node> closedList = new();

                startNode.G = 0;
                startNode.H = Distance(startNode, endNode);
                startNode.F = startNode.G + startNode.H;
                openList.Add(startNode);
                
                while (openList.Count > 0) {
                    int i = LowestF(openList);
                    Node currentNode = openList[i];
                    if (currentNode.Id == endId) {
                        GetPath(startId, endId);
                        return true;
                    }
                    openList.RemoveAt(i);
                    closedList.Add(currentNode);
                    Node neighbour;
                    foreach (Edge edge in currentNode.OutboundEdges) {
                        neighbour = edge.EndNode;
                        if (closedList.Contains(neighbour)) {
                            continue;
                        }
                        var tentativeGScore = currentNode.G + Distance(currentNode, neighbour);
                        if (!openList.Contains(neighbour)) {
                            openList.Add(neighbour);
                        } else if (tentativeGScore >= neighbour.G) {
                            continue;
                        }
                        neighbour.From = currentNode;
                        neighbour.G = tentativeGScore;
                        neighbour.H = Distance(neighbour, endNode);
                        neighbour.F = neighbour.G + neighbour.H;                        
                    }
                }
            }
            return false;
        }
        
        float Distance(Node startNode, Node endNode) {
            return Vector3.SqrMagnitude(startNode.Id.transform.position - endNode.Id.transform.position);
            //return Vector3.Distance(startNode.Id.transform.position, endNode.Id.transform.position);
        }

        int LowestF(List<Node> nodes) {
            int lowestIndex = 0;
            for (int i = 0; i < nodes.Count; i++) {
                if (nodes[i].F < nodes[lowestIndex].F) {
                    lowestIndex = i;
                }
            }
            return lowestIndex;
        }
        
        public void GetPath(GameObject startId, GameObject endId) {
            Node startNode = _nodes.Find(node => node.Id == startId);
            Node endNode = _nodes.Find(node => node.Id == endId);

            if (startNode != null && endNode != null) {
                Paths.Clear();
                Paths.Add(endNode);
                var p = endNode.From;
                while (p != null && p != startNode) {
                    Paths.Insert(0, p);
                    p = p.From;
                }

                Paths.Insert(0, startNode);
            }
        }
    }
}