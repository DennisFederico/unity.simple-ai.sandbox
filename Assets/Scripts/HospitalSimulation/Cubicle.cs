using HospitalSimulation.Goap;
using UnityEngine;

namespace HospitalSimulation {
    public class Cubicle : MonoBehaviour {
        private void Start() {
            World.Instance.AddCubicle(this.gameObject);
        }
    }
}