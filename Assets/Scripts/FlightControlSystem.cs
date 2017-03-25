using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class FlightControlSystem : MonoBehaviour {

    [System.Serializable]
    public class ThrustPotential {
        public float Prograde = 1;
        public float Retrograde = 1;
        public float Horizontal = 1;
        public float Vertical = 1;
    }

    public ThrustPotential ThrusterOutput;
    public PhysicsIntent DesiredMotion;

    private Rigidbody rb;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

	// Update is called once per physics frame
	void FixedUpdate () {

        // MotionIntents are in Inertial space, but Rigidbody wants world-space
        var desiredWorldMotion = new PhysicsIntent {
            Linear = GetComponent<Rigidbody>().transform.TransformDirection(DesiredMotion.Linear),
            Angular = GetComponent<Rigidbody>().transform.TransformDirection(DesiredMotion.Angular)
        };


        var linearDeltaV = desiredWorldMotion.Linear - rb.velocity; // Velocity difference between current and desired
        var angularDeltaV = desiredWorldMotion.Angular - rb.angularVelocity;

        // TODO: should use smoothstep to increase low-delta thrust
        var accel = new PhysicsIntent { Linear = linearDeltaV, Angular = angularDeltaV };

        //rb.AddRelativeForce(accel.Linear, ForceMode.Acceleration);
        //rb.AddRelativeTorque(accel.Angular, ForceMode.Acceleration);

        rb.velocity = Vector3.ClampMagnitude(desiredWorldMotion.Linear, 10);
        rb.angularVelocity = desiredWorldMotion.Angular;


    }
}
