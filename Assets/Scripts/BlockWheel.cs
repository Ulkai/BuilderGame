using UnityEngine;
using System.Collections.Generic;

public class BlockWheel : BlockBase {
    
    Transform pivot;

    public KeyCode forwardKey;
    public KeyCode backwardKey;
    public float velocity = 100;

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

        JointMotor motor = hinge.motor;
        motor.targetVelocity = moveForward;
        hinge.motor = motor;
    }

    public override void Place(Transform socket) {
        transform.position = socket.transform.TransformPoint(-pivot.localPosition);
        transform.rotation = socket.rotation;
    }

    public override void Attach(Transform socket, BlockBase target) {
        target.sockets.Remove(socket);
        hinge.connectedBody = target.GetComponent<Rigidbody>();
    }

    public override void OnPlay() {
        base.OnPlay();
        GetComponent<Rigidbody>().isKinematic = false;
        hinge.useMotor = true;
    }
}
