using UnityEngine;
using System.Collections.Generic;

public class BlockSolid : BlockBase {

    List<Transform> sockets = new List<Transform>();
    Transform pivot;

	// Use this for initialization
	void Start () {
        foreach (Transform child in transform) {
            if (child.name == "Pivot") {
                pivot = child.transform;
            } else {
                sockets.Add(child.transform);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {	
	}

    public override Transform GetClosestSocket(RaycastHit hit) {
        float minDist = Mathf.Infinity;
        Transform closestSocket = null;

        for (int i = 0; i < sockets.Count; i++) {
            float dist = Vector3.Distance(hit.point, sockets[i].position);
            if (minDist > dist) {
                minDist = dist;
                closestSocket = sockets[i];
            }
        }

        return closestSocket;
    }

    public override void Place(Transform socket) {   
        transform.position = socket.transform.TransformPoint(-pivot.localPosition);
        transform.rotation = socket.rotation;
    }
    
    public override void Attach(Transform socket, Transform target) {
        sockets.Remove(socket);
        target.GetComponent<FixedJoint>().connectedBody = GetComponent<Rigidbody>();
    }
}
