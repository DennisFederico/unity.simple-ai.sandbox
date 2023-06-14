using HospitalSimulation.Goap;

namespace HospitalSimulation {
    public class Nurse : GoapAgent {
        new void Start() {
            base.Start();
            SubGoal s1 = new SubGoal("treatPatient", 0, true);
            goals.Add(s1, 3);
        }
    }
}