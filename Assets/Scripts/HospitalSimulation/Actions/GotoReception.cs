using HospitalSimulation.Goap;

namespace HospitalSimulation.Actions {
    public class GotoReception : GoapAction {
        
        public GotoReception() : base("GotoReception") { }
        public override bool PrePerform() {
            return true;
        }

        public override bool PostPerform() {
            return true;
        }
    }
}