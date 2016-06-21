using UnityEngine;
using System.Collections;

public class FPPCamera : MonoBehaviour {

    float yaw;
    float pitch;

    public float sensitivity = 10;
    public float speed = 10;
    public KeyCode forward;
    public KeyCode backward;
    public KeyCode left;
    public KeyCode right;

	// Use this for initialization
	void Start () {	
	}
	
	// Update is called once per frame
	void Update () {

        float x = Input.GetAxis("Mouse X") * sensitivity;
        float y = Input.GetAxis("Mouse Y") * sensitivity;
        
        yaw += x;
        pitch = Mathf.Clamp(pitch - y, -90, 90);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0);

        float moveForward = (Input.GetKey(forward) ? speed : 0) +
            (Input.GetKey(backward) ? -speed : 0);
        float strafe = (Input.GetKey(right) ? speed : 0) +
            (Input.GetKey(left) ? -speed : 0);

        transform.position += (transform.forward * moveForward + transform.right * strafe) * Time.deltaTime;
    }
}
