using HospitalSimulation.Goap;

namespace HospitalSimulation.Actions {
    public class GotoCubicle : GoapAction {
        public GotoCubicle() : base("GotoCubicle") { }

        public override bool PrePerform() {
            return Inventory.TryFindItemWithTag("Cubicle", out target);
        }

        public override bool PostPerform() {
            World.Instance.States.ModifyState("treatingPatient", 1);
            World.Instance.AddCubicle(target);
            Inventory.RemoveItem(target);
            return true;
        }
    }
}