using UnityEngine;

public class DestroyShell : MonoBehaviour {
    [SerializeField] private float timeToLive = 5f;

    void Start() {
        Destroy(this.gameObject, timeToLive);
    }
}