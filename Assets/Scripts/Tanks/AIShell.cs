using System.Linq;
using UnityEngine;

public class AIShell : MonoBehaviour {

    [SerializeField] private GameObject explosion;
    [SerializeField] private string[] tagsToHit = { "Tank", "Terrain" };
    private Rigidbody _rigidbody;

    void OnCollisionEnter(Collision collision) {
        if (tagsToHit.Contains(collision.gameObject.tag)) {
            GameObject exp = Instantiate(explosion, this.transform.position, Quaternion.identity);
            Destroy(exp, 0.5f);
            Destroy(this.gameObject);
        }
    }

    void Start() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update() {
        this.transform.forward = _rigidbody.velocity;
    }
}
