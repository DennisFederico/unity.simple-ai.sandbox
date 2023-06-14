using System.Collections.Generic;
using UnityEngine;

namespace HospitalSimulation.Goap {
    public sealed class World {
        
        public static World Instance { get; } = new ();
        private static readonly Queue<GameObject> Patients = new ();
        private static readonly Queue<GameObject> Cubicles = new ();

        public WorldStates States { get; }

        private World() {
            States = new WorldStates();
            Time.timeScale = 5;
        }
        
        public void AddPatient(GameObject patient) {
            Patients.Enqueue(patient);
        }
        
        public bool TryGetPatient(out GameObject patient) {
            return Patients.TryDequeue(out patient);
        }
        
        public void AddCubicle(GameObject cubicle) {
            Cubicles.Enqueue(cubicle);
            UpdateFreeCubicles();
        }
        
        public bool TryGetCubicle(out GameObject cubicle) {
            var result = Cubicles.TryDequeue(out cubicle);
            if (result) {
                UpdateFreeCubicles();
            }
            return result;
        }
        
        private void UpdateFreeCubicles() {
            States.SetState("freeCubicle", Cubicles.Count);
        }
    }
}