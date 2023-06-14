using System;
using System.Collections.Generic;

namespace HospitalSimulation.Goap {
    
    [Serializable]
    public class WorldState {
        public string key;
        public int value;
    }
    
    public class WorldStates {
        public WorldStates() {
            States = new Dictionary<string, int>();
        }
        
        public WorldStates(WorldStates other) {
            States = new Dictionary<string, int>(other.States);
        }
        
        public WorldStates(Dictionary<string, int> states) {
            States = new Dictionary<string, int>(states);
        }

        public Dictionary<string, int> States { get; }

        public bool HasState(string key) {
            return States.ContainsKey(key);
        }
        
        public void AddState(string key, int value) {
            States.Add(key, value);
        }
        
        public void RemoveState(string key) {
            if (States.ContainsKey(key)) {
                States.Remove(key);
            }
        }

        public void SetState(string key, int value) {
            States[key] = value;
        }
        
        public void ModifyState(string key, int value) {
            if (States.ContainsKey(key)) {
                States[key] += value;
                if (States[key] <= 0) {
                    RemoveState(key);
                }
            } else {
                States.Add(key, value);
            }
        }
    }
}