using UnityEngine;
using System.Collections.Generic;

public class BlockSolid : BlockBase {

    Transform pivot;

	// Use this for initialization
	void Start () {
        foreach (Transform child in transform) {
            if (child.name == "Pivot") {
                pivot = child.transform;
            } else if (child.name.StartsWith("Socket")) {
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
    
    public override void Attach(Transform socket, BlockBase target) {
        target.sockets.Remove(socket);
        GetComponent<Joint>().connectedBody = target.GetComponent<Rigidbody>();
    }

    public override void OnPlay() {
        base.OnPlay();
        GetComponent<Rigidbody>().isKinematic = false;
    }
}
