using System.Linq;
using UnityEngine;

public class Shell : MonoBehaviour {

    [SerializeField] private GameObject explosion;
    [SerializeField] private string[] tagsToHit = { "Tank", "Terrain" };
    
    float speed = 0.0f;
    float ySpeed = 0.0f;
    float mass = 30.0f;
    float force = 4.0f;
    float drag = 1.0f;
    float gravity = -9.8f;
    float gAccel;
    float acceleration;
    
    void OnCollisionEnter(Collision col) {
        if (tagsToHit.Contains(col.gameObject.tag)) {
            GameObject exp = Instantiate(explosion, this.transform.position, Quaternion.identity);
            Destroy(exp, 0.5f);
            Destroy(this.gameObject);
        }
    }

    private void Start() {

        acceleration = force / mass;
        speed += acceleration * 1.0f;
        gAccel = gravity / mass;
    }

    void LateUpdate() {

        speed *= (1 - Time.deltaTime * drag);
        ySpeed += gAccel * Time.deltaTime;
        this.transform.Translate(0.0f, ySpeed, speed);
    }
}
