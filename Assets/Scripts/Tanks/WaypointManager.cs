using System;
using UnityEngine;

namespace Tanks {
    // [ExecuteInEditMode]
    public class WaypointManager : MonoBehaviour {
        [Serializable]
        public struct Link {
            public enum Type {
                OneWay,
                TwoWay
            }

            public GameObject waypointA;
            public GameObject waypointB;
            public Type linkType;
        }

        public static WaypointManager Instance { get; private set; }
        
        [SerializeReference] private GameObject[] waypoints;
        public GameObject[] Waypoints => waypoints;
        public Link[] links;
        public Graph.Graph Graph = new();

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }
            // waypoints = GameObject.FindGameObjectsWithTag(WaypointTag);
        }

        void Start() {
            if (waypoints.Length > 0) {
                foreach (GameObject waypoint in waypoints) {
                    Graph.AddNode(waypoint);
                }

                foreach (Link link in links) {
                    Graph.AddEdge(link.waypointA, link.waypointB);
                    if (link.linkType == Link.Type.TwoWay) {
                        Graph.AddEdge(link.waypointB, link.waypointA);
                    }
                }
            }
        }
        
        // [EditorOnly]
        // private void Update() {
        //     waypoints = GameObject.FindGameObjectsWithTag(WaypointTag);
        //     RenameWaypoints();
        // }

        // void RenameWaypoints() {
        //     int index = 1;
        //     foreach (GameObject waypoint in waypoints) {
        //         waypoint.name = "WP" + $"{index:000}";
        //         index++;
        //     }
        // }
    }
}