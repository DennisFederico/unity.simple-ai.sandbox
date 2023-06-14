using UnityEngine;

namespace Tanks {
    public class FollowWaypointAStar : MonoBehaviour {
        private Graph.Graph _graph;
        private GameObject[] _waypoints;
        private int _currentWaypointIndex = 0;
        private GameObject _currentWaypoint;
        
        private readonly float _waypointSafeDistance = 0.25f;
        [SerializeField] private float moveSpeed = 10.0f;
        [SerializeField] private float rotationSpeed = 10.0f;
        [SerializeField] private float maxTrackerDistance = 3f;

        private GameObject tracker;

        private void Start() {
            _waypoints = WaypointManager.Instance.Waypoints;
            _graph = WaypointManager.Instance.Graph;
            _currentWaypoint = _waypoints[_currentWaypointIndex];
            
            //Create visual for tracking
            tracker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            DestroyImmediate(tracker.GetComponent<SphereCollider>());
            tracker.transform.position = transform.position;
            tracker.transform.rotation = transform.rotation;
            
            Invoke(nameof(GoToRuins),2f);
        }

        private void LateUpdate() {
            ProgressTracker();
            var lookRotation = Quaternion.LookRotation(tracker.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            transform.position += transform.forward * (Time.deltaTime * moveSpeed);
        }

        public void GoToHelipad() {
            _graph.AStar(_currentWaypoint, _waypoints[0]);
            _currentWaypointIndex = 0;
        }
        
        public void GoToRuins() {
            Debug.Log("Going to ruins");
            _graph.AStar(_currentWaypoint, _waypoints[1]);
            _currentWaypointIndex = 0;
        }
        
        private void GoToWaypointIndex(int index) {
            _currentWaypointIndex = index;
        }
        
        void ProgressTracker() {
            
            if (_graph.Paths.Count == 0 || _currentWaypointIndex >= _graph.Paths.Count) return;
            
            if (Vector3.Distance(tracker.transform.position, transform.position) > maxTrackerDistance) return;
                
            if (Vector3.Distance(tracker.transform.position, _graph.Paths[_currentWaypointIndex].Id.transform.position) < _waypointSafeDistance) {
                _currentWaypointIndex++;
                if (_currentWaypointIndex >= _graph.Paths.Count) return;
                _currentWaypoint = _graph.Paths[_currentWaypointIndex].Id;
            }

            tracker.transform.LookAt(_currentWaypoint.transform);
            tracker.transform.position += tracker.transform.forward * (Time.deltaTime * moveSpeed * 1.25f);
        }
    }
}