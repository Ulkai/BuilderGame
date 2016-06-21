using UnityEngine;
using System.Collections;

public class BlockBase : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public virtual Transform GetClosestSocket(RaycastHit hit) {
        return null;
    }

    public virtual void Place(Transform socket) {
        transform.position = socket.position;
        transform.rotation = socket.rotation;
    }

    public virtual void Attach(Transform socket, Transform target) {
    }
}
