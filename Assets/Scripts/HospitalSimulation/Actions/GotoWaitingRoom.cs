using HospitalSimulation.Goap;

namespace HospitalSimulation.Actions {
    public class GotoWaitingRoom : GoapAction {
        
        public GotoWaitingRoom() : base("GotoWaitingRoom") { }
        public override bool PrePerform() {
            return true;
        }

        public override bool PostPerform() {
            World.Instance.States.ModifyState("patientWaiting", 1);
            World.Instance.AddPatient(gameObject);
            Beliefs.ModifyState("atHospital", 1);
            return true;
        }
    }
}