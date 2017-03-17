using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObserverController : MonoBehaviour {

    public float cameraSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
    void FixedUpdate()
    {
            
    }

	// Update is called once per frame
	void Update () {
        Vector3 move = new Vector3();
        move += Input.GetAxis("Horizontal") * GetComponent<Transform>().right;
        move += Input.GetAxis("Vertical") * GetComponent<Transform>().forward;
        move *= cameraSpeed * Time.deltaTime;

        GetComponent<CharacterController>().Move(move);

        Vector3 rotate = new Vector3();
        rotate.z = Input.GetAxis("Roll");
        

        Debug.Log("Move: " + move);
    }
}
