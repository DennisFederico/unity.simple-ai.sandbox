using TMPro;
using UnityEngine;

namespace Tanks {
    [ExecuteInEditMode]
    public class WaypointDebug : MonoBehaviour {
        void Update() {
            GetComponent<TextMeshPro>().text = this.transform.parent.gameObject.name;
        }
    }
}