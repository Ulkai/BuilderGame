using UnityEngine;
using System.Collections;

public class TPPCamera : MonoBehaviour {

    float yaw;
    float pitch;
    Vector3 moveSpeed;
    Vector3 currentMoveAcceleration;
    bool freeLook = false;


    public float sensitivity = 10;
    public float scrollSensitivity = 10;
    public float speed = 10;
    public float zoomMin = 1;
    public float zoomMax = 15;
    public KeyCode forwardKey = KeyCode.W;
    public KeyCode backwardKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;

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

        Vector3 impulse = new Vector3(
            (Input.GetKey(forwardKey) ? 1 : 0) + (Input.GetKey(backwardKey) ? -1 : 0),
            (Input.GetKey(rightKey) ? 1 : 0) + (Input.GetKey(leftKey) ? -1 : 0),
            0);

        if (impulse.x != 0) {
            moveSpeed.x = impulse.x;
        } 

        if (impulse.y != 0) {
            moveSpeed.y = impulse.y;
        } 

        Vector3 forward = Vector3.Cross(transform.right, Vector3.up);
        transform.position += (forward * moveSpeed.x + transform.right * moveSpeed.y) * speed * Time.deltaTime;

        impulse.z = Input.GetAxis("Mouse ScrollWheel");

        if (impulse.z != 0) {
            moveSpeed.z = impulse.z;
        }

        float zoom = cameraTransform.localPosition.z + moveSpeed.z * scrollSensitivity;
        zoom = Mathf.Clamp(zoom, -zoomMax, -zoomMin);

        cameraTransform.localPosition = new Vector3(0, 0, zoom);

        moveSpeed = Vector3.SmoothDamp(moveSpeed, Vector3.zero, ref currentMoveAcceleration, 0.1f);
    }
}
