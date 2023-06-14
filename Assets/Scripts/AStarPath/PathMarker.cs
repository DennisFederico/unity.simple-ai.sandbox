using UnityEngine;

namespace AStarPath {
    public class PathMarker : MonoBehaviour {
        [SerializeField] private TextMesh GText;
        [SerializeField] private TextMesh HText;
        [SerializeField] private TextMesh FText;
        
        public void SetValueForDisplay(float g, float h, float f) {
            GText.text = $"G:{g:F1}";
            HText.text = $"H:{h:F1}";
            FText.text = $"F:{f:F1}";
        }
    }
}