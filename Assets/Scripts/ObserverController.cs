using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObserverController : MonoBehaviour {

    public float cameraSpeed;

    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
    }
	
    void FixedUpdate()
    {
            
    }

	// Update is called once per frame
	void Update () {
        Vector3 move = new Vector3();
        move += Input.GetAxis("Horizontal") * Vector3.right;
        move += Input.GetAxis("Vertical") * Vector3.forward;
        move *= cameraSpeed;

        rb.velocity = transform.TransformDirection(move);

        Vector3 rotate = new Vector3();
        rotate.z = -Input.GetAxis("Roll");

        if ( Input.GetMouseButtonDown(2)) { // Middle Mouse toggles mouse capture
            if ( Cursor.lockState != CursorLockMode.Locked ) {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            } else {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        if (Cursor.lockState == CursorLockMode.Locked) {
            rotate.x = -Input.GetAxisRaw("Mouse Y");
            rotate.y = Input.GetAxisRaw("Mouse X");
        }

        rb.angularVelocity = transform.TransformDirection(rotate);
    }
}
