using UnityEngine;

namespace HospitalSimulation {
    public class PatientSpawner : MonoBehaviour {
        [SerializeField] private GameObject patientPrefab;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private float spawnInterval = 5f;
        [SerializeField] private int maxPatients = 10;
        private float _spawnTimer;
        private int _patientCount;

        private void Start() {
            _spawnTimer = spawnInterval;
        }

        private void Update() {
            if (_patientCount >= maxPatients) return;
            _spawnTimer -= Time.deltaTime;
            if (_spawnTimer < 0) {
                _spawnTimer += spawnInterval;
                _patientCount++;
                Instantiate(patientPrefab, spawnPoint.position, Quaternion.identity);
            }
        }
    }
}