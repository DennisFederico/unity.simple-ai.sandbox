using HospitalSimulation.Goap;
using UnityEngine;

namespace HospitalSimulation.AgentVisualiser {
    [ExecuteInEditMode]
    public class GoapAgentVisual : MonoBehaviour {
        public GoapAgent thisAgent;

        // Start is called before the first frame update
        void Start() {
            thisAgent = this.GetComponent<GoapAgent>();
        }

        // Update is called once per frame
        void Update() { }
    }
}