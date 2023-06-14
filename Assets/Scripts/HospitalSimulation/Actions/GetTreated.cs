using HospitalSimulation.Goap;

namespace HospitalSimulation.Actions {
    public class GetTreated : GoapAction {
        
        public GetTreated(string actionName) : base("GetTreated") { }
        public override bool PrePerform() {
            return Inventory.TryFindItemWithTag("Cubicle", out target);
        }

        public override bool PostPerform() {
            World.Instance.States.ModifyState("Treated", 1);
            Beliefs.ModifyState("isCuredBelief", 1);
            Inventory.RemoveItem(target);
            return true;
        }
    }
}