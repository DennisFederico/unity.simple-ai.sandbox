using System.Collections.Generic;
using UnityEngine;

namespace HospitalSimulation.Goap {
    public class GoapInventory {
        List<GameObject> items = new();
        
        public void AddItem(GameObject item) {
            items.Add(item);
        }
        
        public void RemoveItem(GameObject item) {
            items.Remove(item);
        }
        
        public bool TryFindItemWithTag(string tag, out GameObject item) {
            item = items.Find(item => item.CompareTag(tag));
            return item != null;
        }
    }
}