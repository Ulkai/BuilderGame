using UnityEngine;
using System.Collections.Generic;

public class BlockLeg : BlockBase {

    Transform pivot;

    public KeyCode forwardKey;
    public KeyCode backwardKey;
    public float velocity = 100;
    public bool invertedDrive = false;
    public const float SQRT_2_HALF = 0.70710678118f;

    ConfigurableJoint configurableJoint;

    // Use this for initialization
    void Start() {
        foreach (Transform child in transform) {
            if (child.name == "Pivot") {
                pivot = child.transform;
            }
        }
        configurableJoint = GetComponent<ConfigurableJoint>();
    }

    // Update is called once per frame
    void Update() {
        float moveForward =
            (Input.GetKey(forwardKey) ? velocity : 0) +
            (Input.GetKey(backwardKey) ? -velocity : 0);
        if (invertedDrive) {
            moveForward = -moveForward;
        }
        float t = Time.realtimeSinceStartup;
        configurableJoint.targetPosition = new Vector3(0, Mathf.Sin(t), Mathf.Cos(t));
/*
        JointMotor motor = configurableJoint.motor;
        motor.targetVelocity = moveForward;
        configurableJoint.motor = motor; 
        */
    }

    public override void Place(Transform socket) {
        transform.position = socket.transform.TransformPoint(-pivot.localPosition);
        transform.rotation = socket.rotation;
        Vector3 forward = transform.forward;
        invertedDrive = (forward.z > SQRT_2_HALF);
    }

    public override void Attach(Transform socket, BlockBase target) {
        target.sockets.Remove(socket);
        configurableJoint.connectedBody = target.GetComponent<Rigidbody>();
    }

    public override void OnPlay() {
        base.OnPlay();
        GetComponent<Rigidbody>().isKinematic = false;
//        configurableJoint.useMotor = true;
    }
}
