using UnityEngine;
using System.Collections.Generic;

public class BlockWheel : BlockBase {
    
    Transform pivot;

    public KeyCode forwardKey;
    public KeyCode backwardKey;
    public float velocity = 100;
    public bool invertedDrive = false;
    public const float SQRT_2_HALF = 0.70710678118f;

    HingeJoint hinge;

    // Use this for initialization
    void Start() {
        foreach (Transform child in transform) {
            if (child.name == "Pivot") {
                pivot = child.transform;
            }
        }
        hinge = GetComponent<HingeJoint>();
    }

    // Update is called once per frame
    void Update() {
        float moveForward =
            (Input.GetKey(forwardKey) ? velocity : 0) +
            (Input.GetKey(backwardKey) ? -velocity : 0);
        if (invertedDrive) {
            moveForward = -moveForward;
        }

        JointMotor motor = hinge.motor;
        motor.targetVelocity = moveForward;
        hinge.motor = motor;
    }

    public override void Place(Transform socket) {
        transform.position = socket.transform.TransformPoint(-pivot.localPosition);
        transform.rotation = socket.rotation;
        Vector3 forward = transform.forward;
        invertedDrive = (forward.z > SQRT_2_HALF);
    }

    public override void Attach(BlockBase target) {
        hinge.connectedBody = target.GetComponent<Rigidbody>();
    }

    public override void OnPlay(bool enable) {
        base.OnPlay(enable);
        GetComponent<Rigidbody>().isKinematic = !enable;
        hinge.useMotor = enable;
    }
}
