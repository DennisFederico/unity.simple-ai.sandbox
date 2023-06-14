using HospitalSimulation.Goap;
using UnityEngine;
using UnityEngine.UI;

namespace HospitalSimulation {
    public class UpdateWorld : MonoBehaviour {
        
        [SerializeField] private Text worldStatesText;

        private void LateUpdate() {
            worldStatesText.text = "";
            foreach (var state in World.Instance.States.States) {
                worldStatesText.text += $"{state.Key}: {state.Value}\n";
            }
        }
    }
}