using HospitalSimulation.Goap;

namespace HospitalSimulation.Actions {
    public class ReturnHome : GoapAction {
        public ReturnHome() : base("ReturnHome") { }
        public override bool PrePerform() {
            return true;
        }

        public override bool PostPerform() {
            return true;
        }
    }
}