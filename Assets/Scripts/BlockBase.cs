using UnityEngine;
using System.Collections.Generic;

public class BlockBase : MonoBehaviour {

    public List<Transform> sockets = new List<Transform>();

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

    public virtual void Attach(BlockBase target) {
    }

    public virtual void OnPlay(bool enable) {
    }
}
