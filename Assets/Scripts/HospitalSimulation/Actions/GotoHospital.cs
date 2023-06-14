using HospitalSimulation.Goap;

namespace HospitalSimulation.Actions {
    public class GotoHospital : GoapAction {
        
        public GotoHospital() : base("GotoHospital") { }
        public override bool PrePerform() {
            return true;
        }

        public override bool PostPerform() {
            return true;
        }
    }
}