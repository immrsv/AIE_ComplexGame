using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class NavigationControlSystem : MonoBehaviour {
    
    public enum NavigationMode
    {
        Idle,
        Seek,
        Evade,
        Orbit
    }

    public bool AvoidObstacles;

    public Vector3 _CurrentWaypoint;

    private GameObject Target;

    private NavigationMode Mode;

    private FlightControlSystem FCS;

    private Stack<Vector3> Waypoints = new Stack<Vector3>();
    


    public bool IsArrived
    {
        get
        {
            return Target == null; // No Target, Has arrived
        }
    }

    private bool IsWaypointReached { get { return (Waypoints.Count == 0 || (Waypoints.Peek() - transform.position).magnitude < 1.0f); } }

    public void SetIdle() {
        DoArrived();
    }

    public void SetNavTask( GameObject target, NavigationMode mode = NavigationMode.Seek) {
        Target = target;
        Mode = mode;
        CreateWaypoints();
    }

	// Use this for initialization
	void Start () {
        FCS = GetComponent<FlightControlSystem>();
	}
	
    public void OnTriggerEnter( Collider other ) {
        if (Target == null || Target.transform != (other.transform.root ?? other.transform)) {
            // Target is not a defined object or Collided with something other than target, not relevant here

        } else {
            if (Mode == NavigationMode.Evade) return; // Contacted Target, but trying to evade (apparently unsuccessfully), can't "arrive" at target.

            Debug.Log(gameObject.name + "Arrived At : " + (other.transform.root ?? other.transform).name);

            // Arrived
            DoArrived();
        }
    }

	
	void Update () {

        //if (IsArrived)
        //    return;

        if (IsWaypointReached)
            DoWaypointReached();
       
        switch (Mode)
        {
            case NavigationMode.Idle:
                DoIdle();
                break;
            case NavigationMode.Seek:
                DoSeek();
                break;
            case NavigationMode.Evade:
                DoEvade();
                break;
            case NavigationMode.Orbit:
                DoOrbit();
                break;
        }
	}

    private void DoIdle() {
        //FCS.DesiredMotion = new PhysicsIntent();
    }

    private void DoSeek()
    {
        if ( Waypoints.Count == 0 ) {
            Debug.LogWarning("NavigationControlSystem on " + gameObject.name + " is Seeking without Waypoints.");
            return;
        }

        var desiredMotion = new PhysicsIntent();

        var targetPosn = Waypoints.Peek();
        var targetDirn = targetPosn - transform.position;
        var speed = (Waypoints.Count == 1) ? (targetPosn - transform.position).magnitude : 10.0f;

        targetDirn.Normalize();
        targetDirn.Scale(new Vector3(speed, speed, speed));

        var targetLocalDirn = transform.InverseTransformDirection(targetDirn);
        var targetLocalRotn = (Quaternion.FromToRotation(Vector3.forward, targetLocalDirn)).eulerAngles;
        targetLocalRotn.Scale(new Vector3(0.2f, 0.2f, 0.2f));

        desiredMotion.Linear = targetLocalDirn;
        desiredMotion.Angular = targetLocalRotn;

        FCS.DesiredMotion = desiredMotion;

        FCS.MaxSpeed = (targetDirn.magnitude > 75) ? 50 : 10;
    }

    private void DoEvade()
    {
        Debug.LogWarning("NavigationControlSystem::DoEvade() - Not Implemented");
    }

    private void DoOrbit()
    {
        Debug.LogWarning("NavigationControlSystem::DoOrbit() - Not Implemented");
    }

    private void DoArrived() {
        Waypoints.Clear();
        Target = null;
        Mode = NavigationMode.Idle;

        FCS.DesiredMotion = new PhysicsIntent();
    }

    private void DoWaypointReached() {
        if (Waypoints.Count > 0) {
            Waypoints.Pop();
            _CurrentWaypoint = (Waypoints.Count > 0) ? Waypoints.Peek() : new Vector3(float.NaN, float.NaN, float.NaN);
        }
    }

    private void CreateWaypoints() {
        Debug.Log("NavigationControlSystem::CreateWaypoints() - BEGIN");

        var path = new Stack<Vector3>();
        path.Push(transform.position);

        if (AvoidObstacles) {

            var complexityThreshold = 50;
            var hit = false;
            var hitInfo = new RaycastHit();
            var lastHit = transform;

            do {
                var ray = new Ray(path.Peek(), (Target.transform.position - path.Peek()).normalized);
                hit = Physics.Raycast(ray, out hitInfo, (path.Peek() - Target.transform.position).magnitude);

                hit = hit && (hitInfo.transform.parent ?? hitInfo.transform).gameObject != Target;

                if (hit) {

                    // generate offset
                    var hitOffset = hitInfo.point - hitInfo.transform.position;

                    var dirToHit = hitOffset.normalized;
                    var dirToObj = (hitInfo.transform.position - path.Peek()).normalized;

                    var hitAngle = Mathf.Acos(Vector3.Dot(dirToHit, -dirToObj));
                    var hitAxis = Vector3.Cross(dirToHit, -dirToObj).normalized;
                    
                    float offsetScale = hitOffset.magnitude + 3.0f;

                    if (hitInfo.transform == lastHit) {
                        offsetScale += (path.Pop() - hitInfo.point).magnitude * 0.5f;
                        Debug.Log("NavigationControlSystem::CreateWaypoints() - Re-evaluating: " + hitInfo.transform.gameObject.name);
                    } else {
                        Debug.Log("NavigationControlSystem::CreateWaypoints() - Avoiding: " + hitInfo.transform.gameObject.name);
                    }

                    var offset = Vector3.Cross(-dirToObj, hitAxis).normalized;
                    offset.Scale(new Vector3(offsetScale, offsetScale, offsetScale));

                    lastHit = hitInfo.transform;
                    path.Push(hitInfo.point + offset);
                }

                if (path.Count > 20 ) {
                    hit = false;
                    Debug.LogWarning("NavigationControlSystem::CreateWaypoints() - Path Too Complex!");

                }
            } while (hit && --complexityThreshold >= 0);
        }

        path.Push(Target.transform.position);

        // reverse order and transfer to Waypoints stack
        Waypoints.Clear();
        while (path.Count > 0)
            Waypoints.Push(path.Pop());

        _CurrentWaypoint = (Waypoints.Count > 0) ? Waypoints.Peek() : new Vector3(float.NaN, float.NaN, float.NaN);
        Debug.Log("NavigationControlSystem::CreateWaypoints() - END");
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan;
        foreach (Vector3 v in Waypoints) {
            Gizmos.DrawSphere(v, 0.5f);
        }
    }
}
