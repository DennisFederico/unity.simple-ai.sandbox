using UnityEngine;

namespace Tanks {
    public class FollowWaypoint : MonoBehaviour {
        private GameObject[] waypoints;
        private int _currentWaypointIndex = 0;
        private readonly float _waypointSafeDistance = 0.25f;
        [SerializeField] private float moveSpeed = 10.0f;
        [SerializeField] private float rotationSpeed = 10.0f;
        [SerializeField] private float maxTrackerDistance = 3f;

        private GameObject tracker;

        private void Start() {
            //Create visual for tracking
            tracker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            DestroyImmediate(tracker.GetComponent<SphereCollider>());
            tracker.transform.position = transform.position;
            tracker.transform.rotation = transform.rotation;

            waypoints = WaypointManager.Instance.Waypoints;
        }

        private void Update() {
            ProgressTracker();
            var lookRotation = Quaternion.LookRotation(tracker.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            transform.position += transform.forward * (Time.deltaTime * moveSpeed);
        }

        void ProgressTracker() {
            if (Vector3.Distance(tracker.transform.position, transform.position) > maxTrackerDistance) return;
                
            if (Vector3.Distance(tracker.transform.position, waypoints[_currentWaypointIndex].transform.position) < _waypointSafeDistance) {
                _currentWaypointIndex = (_currentWaypointIndex + 1) % waypoints.Length;
            }

            tracker.transform.LookAt(waypoints[_currentWaypointIndex].transform);
            tracker.transform.position += tracker.transform.forward * (Time.deltaTime * moveSpeed * 1.25f);
        }
    }
}