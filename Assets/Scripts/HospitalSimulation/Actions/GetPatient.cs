using HospitalSimulation.Goap;
using UnityEngine;

namespace HospitalSimulation.Actions {
    public class GetPatient : GoapAction {
        private GameObject _cubicle;

        public GetPatient() : base("GetPatient") { }

        public override bool PrePerform() {
            Debug.Log($"PrePerform {name}");
            if (World.Instance.TryGetPatient(out target) &&
                World.Instance.TryGetCubicle(out _cubicle)) {
                Inventory.AddItem(_cubicle);
                Debug.Log($"PrePerform success - P:{target.name} C:{_cubicle.name}");
                return true;
            }

            Debug.Log($"PrePerform fail - P:{target != null } C:{_cubicle != null}");
            if (target != null) {
                World.Instance.AddPatient(target);
                target = null;
            }
            return false;
        }

        public override bool PostPerform() {
            World.Instance.States.ModifyState("patientWaiting", -1);
            if (target) {
                target.GetComponent<GoapAgent>().Inventory.AddItem(_cubicle);
            }
            return true;
        }
    }
}