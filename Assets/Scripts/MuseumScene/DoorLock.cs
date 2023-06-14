using UnityEngine;

namespace MuseumScene {
    public class DoorLock : MonoBehaviour {
        public bool isLocked = false;
        [Range(0f, 100f)] public float pickLockLevel;
        
        public bool TryPickLock(float pickLockLevel) {
            if (isLocked) {
                if (pickLockLevel >= this.pickLockLevel) {
                    isLocked = false;
                    Debug.Log("Door pick-locked!");
                    return true;
                }
            }
            return false;
        }
    }
}