using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drive : MonoBehaviour {
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float rotationSpeed = 15.0f;
    // [Range(-45, 15)] [SerializeField] private float minTurretInclination = -45;
    // [Range(-45, 15)] [SerializeField] private float maxTurretInclination = 15;
    [SerializeField] private Transform turretTransform;
    [SerializeField] private Transform nozzleReference;
    [SerializeField] private GameObject bulletPrefab;

    void Update() {
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        
        translation *= speed * Time.deltaTime;
        rotation *= rotationSpeed * Time.deltaTime;

        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);

        float inclinationDelta = 0f;
        if (Input.GetKey(KeyCode.R)) {
            inclinationDelta -= 0.5f;
        } else if (Input.GetKey(KeyCode.F)) {
            inclinationDelta += 0.5f;
        } else if (Input.GetKeyDown(KeyCode.Space)) {
            Instantiate(bulletPrefab, nozzleReference.position, nozzleReference.rotation);
        }
        turretTransform.Rotate(Vector3.right, inclinationDelta);
        // Debug.Log($"Inclination: {turretTransform.localEulerAngles.x}");
        // Debug.Log($"Inclination: {turretTransform.eulerAngles.x}");
    }
}
