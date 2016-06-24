using UnityEngine;
using System.Collections;

public class PhysicsMover : MonoBehaviour {

    public float speed = 2.0f;
    Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        //rb.position -= transform.forward * speed * Time.deltaTime;
        rb.MovePosition(rb.position + transform.forward * speed * Time.deltaTime);
    }
}
