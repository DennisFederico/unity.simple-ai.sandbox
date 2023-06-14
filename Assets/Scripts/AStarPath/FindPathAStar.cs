using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AStarPath {
    public class AStarPathMarker {
        public MapLocation Location { get; set; }
        public float G { get; set; } //COST FROM START
        public float H { get; set; } //COST TO GOAL
        public float F { get; set; } //G + H = TOTAL COST
        public AStarPathMarker Parent { get; set; }
        public GameObject MarkerGameObject { get; set; }

        public override bool Equals(object obj) {
            return obj is AStarPathMarker marker && Location.Equals(marker.Location);
        }

        public override int GetHashCode() {
            return Location.GetHashCode();
        }
    }

    public class FindPathAStar : MonoBehaviour {
        [SerializeField] private Maze maze;
        [SerializeField] private Material closedMaterial;
        [SerializeField] private Material openMaterial;
        [SerializeField] private GameObject startMarker;
        [SerializeField] private GameObject endMarker;
        [SerializeField] private GameObject pathMarker;

        private List<AStarPathMarker> _closedMarkers = new();
        private List<AStarPathMarker> _openMarkers = new();

        private AStarPathMarker _goalMarker;
        private AStarPathMarker _startMarker;
        private AStarPathMarker _lastMarker;
        private bool _isDone = true;
        private bool _pathShown = false;

        private void RemoveAllMarkers() {
            var markersInScene = GameObject.FindGameObjectsWithTag("marker");
            foreach (var marker in markersInScene) {
                Destroy(marker);
            }
        }

        private void BeginSearch() {
            _isDone = false;
            _pathShown = false;
            RemoveAllMarkers();

            List<MapLocation> locations = new();
            for (int z = 0; z < maze.depth - 1; z++) {
                for (int x = 0; x < maze.width; x++) {
                    //Walls are represented by '1's in the map array
                    if (maze.Map[x, z] != 1) {
                        locations.Add(new MapLocation(x, z));
                    }
                }
            }

            locations.Shuffle();

            _startMarker = new AStarPathMarker() {
                Location = new MapLocation(locations[0].X, locations[0].Z),
                G = 0,
                H = 0,
                F = 0,
                Parent = null,
                MarkerGameObject = Instantiate(startMarker,
                    new Vector3(locations[0].X * maze.scale, 0, locations[0].Z * maze.scale),
                    Quaternion.identity)
            };

            _goalMarker = new AStarPathMarker() {
                Location = new MapLocation(locations[1].X, locations[1].Z),
                G = 0,
                H = 0,
                F = 0,
                Parent = null,
                MarkerGameObject = Instantiate(endMarker,
                    new Vector3(locations[1].X * maze.scale, 0, locations[1].Z * maze.scale),
                    Quaternion.identity)
            };

            _closedMarkers.Clear();
            _openMarkers.Clear();
            _openMarkers.Add(_startMarker);
            _lastMarker = _startMarker;
        }

        void Search(AStarPathMarker thisMarker) {
            if (thisMarker == null) return;
            if (thisMarker.Equals(_goalMarker)) {
                _isDone = true;
                return;
            }

            //Search top, right, bottom, left
            foreach (var direction in maze.Directions) {
                var nextMarker = direction + thisMarker.Location;
                if (maze.Map[nextMarker.X, nextMarker.Z] == 1) continue; //Its a wall
                if (!nextMarker.IsWithinMaze(maze)) continue; //Its outside the maze
                if (_closedMarkers.Contains(new AStarPathMarker() { Location = nextMarker })) continue; //Its already closed

                //New distance from start to this marker
                float G = Vector2.Distance(thisMarker.Location.ToVector(), nextMarker.ToVector()) + thisMarker.G;
                //Distance to goal from this marker
                float H = Vector2.Distance(nextMarker.ToVector(), _goalMarker.Location.ToVector());
                //Total distance
                float F = G + H;

                var openMarker = _openMarkers.Find(m => m.Location.Equals(nextMarker));
                if (openMarker == default) {
                    //Create a new marker
                    GameObject nextMarkerGameObject = Instantiate(pathMarker,
                        new Vector3(nextMarker.X * maze.scale, 0, nextMarker.Z * maze.scale),
                        Quaternion.identity);
                    nextMarkerGameObject.GetComponent<PathMarker>().SetValueForDisplay(G, H, F);
                    _openMarkers.Add(new AStarPathMarker() {
                        Location = nextMarker,
                        G = G,
                        H = H,
                        F = F,
                        MarkerGameObject = nextMarkerGameObject,
                        Parent = thisMarker
                    });
                } else if (openMarker.F > F) {
                    //Update the marker if this path is shorter
                    openMarker.G = G;
                    openMarker.H = H;
                    openMarker.F = F;
                    openMarker.Parent = thisMarker;
                    openMarker.MarkerGameObject.GetComponent<PathMarker>().SetValueForDisplay(G, H, F);
                }
            }

            _openMarkers = _openMarkers.OrderBy(marker => marker.F).ThenBy(marker => marker.H).ToList();
            var aStarPathMarker = _openMarkers.ElementAt(0);
            _closedMarkers.Add(aStarPathMarker);
            _openMarkers.Remove(aStarPathMarker);
            aStarPathMarker.MarkerGameObject.GetComponent<Renderer>().material = closedMaterial;
            _lastMarker = aStarPathMarker;
        }

        void DrawPath(AStarPathMarker marker) {
            RemoveAllMarkers();
            if (marker == null) return;
            DrawPath(marker.Parent);
            //Create a new marker GameObject
            GameObject markerGameObject = Instantiate(pathMarker,
                new Vector3(marker.Location.X * maze.scale, 0, marker.Location.Z * maze.scale),
                Quaternion.identity);
            markerGameObject.GetComponent<PathMarker>().SetValueForDisplay(marker.G, marker.H, marker.F);
            marker.MarkerGameObject.GetComponent<Renderer>().material = openMaterial;
            marker.MarkerGameObject = markerGameObject;
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.A)) {
                if (!_isDone) {
                    Search(_lastMarker);
                } else {
                    if (_pathShown) {
                        BeginSearch();
                    } else {
                        DrawPath(_lastMarker);
                        _pathShown = true;
                    }
                }
            }
        }
    }
}