using UnityEngine;
using System.Collections;

public class TPPCamera : MonoBehaviour {

    float yaw;
    float pitch;
    bool freeLook = false;

    public float sensitivity = 10;
    public float scrollSensitivity = 10;
    public float speed = 10;
    public float zoomMin = 1;
    public float zoomMax = 15;
    public KeyCode forwardKey;
    public KeyCode backwardKey;
    public KeyCode leftKey;
    public KeyCode rightKey;

    Transform cameraTransform;

	// Use this for initialization
	void Start () {
        cameraTransform = GetComponentInChildren<Camera>().transform;
        Vector3 eulerAngles = transform.eulerAngles;
        pitch = eulerAngles.x;
        yaw = eulerAngles.y;
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(1)) {
            Cursor.lockState = CursorLockMode.Locked;
            freeLook = true;
        }

        if (Input.GetMouseButtonUp(1)) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            freeLook = false;
        }

        if (freeLook) {
            float x = Input.GetAxis("Mouse X") * sensitivity;
            float y = Input.GetAxis("Mouse Y") * sensitivity;

            yaw += x;
            pitch = Mathf.Clamp(pitch - y, -90, 90);

            transform.eulerAngles = new Vector3(pitch, yaw, 0);
        }

        float moveForward = 
            (Input.GetKey(forwardKey) ? speed : 0) +
            (Input.GetKey(backwardKey) ? -speed : 0);
        float strafe = 
            (Input.GetKey(rightKey) ? speed : 0) +
            (Input.GetKey(leftKey) ? -speed : 0);

        Vector3 forward = Vector3.Cross(transform.right, Vector3.up);

        transform.position += (forward * moveForward + transform.right * strafe) * Time.deltaTime;

        float zoom = cameraTransform.localPosition.z + Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity;
        zoom = Mathf.Clamp(zoom, -zoomMax, -zoomMin);

        cameraTransform.localPosition = new Vector3(0, 0, zoom);
    }
}
