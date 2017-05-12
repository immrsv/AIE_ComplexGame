using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class NavigationControlSystem : MonoBehaviour {

    [System.Flags]
    public enum NavigationMode {
        Idle = 1,
        Hold = 2,
        Seek = 4,
        ContinuousSeek = 8,
        MoveTo = 16,
        Pursue = 32,
        Evade = 64,
        Orbit = 128
    }

    public bool AvoidObstacles;
    public float ReseekDelay = 1.0f;

    public float CruiseSpeed = 50;
    public float CruiseRange = 75;
    public float GeneralSpeed = 10;

    private GameObject Target;

    private float TargetRadius;
    private float TargetSqrRadius;

    private NavigationMode Mode;

    private FlightControlSystem FCS;

    private Vector3 Waypoint;

    private float ReseekAfter;

    public bool IsArrived {
        get {
            if (Target == null)
                return true;

            // Continuous Seek or Evade can't "Arrive" at their target
            if (Mode == NavigationMode.ContinuousSeek || Mode == NavigationMode.Evade)
                return false;
            
            return (Target.transform.position - transform.position).sqrMagnitude <= TargetSqrRadius;
        }
    }


    private bool IsWaypointReached {
        get {
            return Target == null || (Waypoint - transform.position).sqrMagnitude <= 0.5;
        }
    }

    public void SetIdle() {
        Target = null;

        DoArrived();
        FCS.DesiredMotion = new PhysicsIntent();
    }

    public void SetHold() {
        Mode = NavigationMode.Hold;
        FCS.DesiredMotion = new PhysicsIntent { Linear = -FCS.Drift };
    }

    public void SetNavTask(GameObject target, float radius = 1f, NavigationMode mode = NavigationMode.Seek) {
        Target = target;

        TargetRadius = radius;
        TargetSqrRadius = radius * radius;
        Mode = mode;

        UpdateWaypoint();
    }

    // Use this for initialization
    void Start() {
        FCS = GetComponent<FlightControlSystem>();
    }

    public void OnTriggerEnter(Collider other) {
        if (Target == null || Target.transform != (other.transform.root ?? other.transform)) {
            // Target is not a defined object or Collided with something other than target, not relevant here

        } else {
            if (Mode == NavigationMode.Evade) return; // Contacted Target, but trying to evade (apparently unsuccessfully), can't "arrive" at target.

            //Debug.Log(gameObject.name + "Arrived At : " + (other.transform.root ?? other.transform).name);

            // Arrived
            DoArrived();
        }
    }


    void Update() {

        if (IsArrived) {
            DoArrived();
            return;
        }

        if (IsWaypointReached) {
            DoWaypointReached();
        }


        switch (Mode) {
            case NavigationMode.Idle:
                DoIdle();
                break;
            case NavigationMode.Hold:
                DoHold();
                break;
            case NavigationMode.Seek:
                DoSeek();
                break;
            case NavigationMode.ContinuousSeek:
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
        FCS.DesiredMotion = new PhysicsIntent();
    }

    private void DoHold() {
        FCS.DesiredMotion = new PhysicsIntent { Linear = -FCS.Drift };
    }

    private void DoSeek() {

        if (Time.realtimeSinceStartup >= ReseekAfter) {
            UpdateWaypoint();
        }


        var desiredMotion = new PhysicsIntent();

        var targetPosn = Waypoint;
        var targetDirn = targetPosn - transform.position;
        var speed = 10.0f;

        targetDirn.Normalize();
        targetDirn.Scale(new Vector3(speed, speed, speed));

        var targetLocalDirn = transform.InverseTransformDirection(targetDirn);
        var targetLocalRotn = (Quaternion.FromToRotation(Vector3.forward, targetLocalDirn)).eulerAngles;
        targetLocalRotn.Scale(new Vector3(0.2f, 0.2f, 0.2f));

        desiredMotion.Linear = targetLocalDirn;
        desiredMotion.Angular = targetLocalRotn;

        FCS.DesiredMotion = desiredMotion;

        FCS.MaxSpeed = (targetDirn.magnitude > CruiseRange) ? CruiseSpeed : GeneralSpeed;
    }

    private void DoEvade() {

        if (Time.realtimeSinceStartup >= ReseekAfter) {
            UpdateWaypoint();
        }


        var desiredMotion = new PhysicsIntent();

        var targetPosn = Waypoint;
        var targetDirn = targetPosn - transform.position;
        var speed = 10.0f;

        targetDirn.Normalize();
        targetDirn.Scale(new Vector3(speed, speed, speed));

        var targetLocalDirn = transform.InverseTransformDirection(targetDirn);
        var targetLocalRotn = (Quaternion.FromToRotation(Vector3.forward, targetLocalDirn)).eulerAngles;
        targetLocalRotn.Scale(new Vector3(0.2f, 0.2f, 0.2f));

        desiredMotion.Linear = targetLocalDirn;
        desiredMotion.Angular = targetLocalRotn;

        FCS.DesiredMotion = desiredMotion;

        FCS.MaxSpeed = (targetDirn.magnitude > CruiseRange) ? CruiseSpeed : GeneralSpeed;

    }

    private void DoOrbit() {
        Debug.LogWarning("NavigationControlSystem::DoOrbit() - Not Implemented");
    }

    private void DoArrived() {
        Target = null;
        Mode = NavigationMode.Hold;
    }

    private void DoWaypointReached() {
        UpdateWaypoint();
    }

    private void UpdateWaypoint() {
        ReseekAfter = Time.realtimeSinceStartup + ReseekDelay;

        if ( Target == null ) {
            Debug.LogError(gameObject.name + " attempting to path without target");
            return;
        }
        
        if ((Mode.HasAny(NavigationMode.Seek | NavigationMode.ContinuousSeek))) {
            //Debug.Log(gameObject.name + " :: NCS :: waypointing toward: " + Target.name);
            var offset = (transform.position - Target.transform.position).normalized;
            offset.Scale(new Vector3(TargetRadius * 0.9f, TargetRadius * 0.9f, TargetRadius * 0.9f));

            Waypoint = Target.transform.position + offset;

        } else {
            //Debug.Log(gameObject.name + " :: NCS :: Evading: " + Target.name);
            var offset = (transform.position - Target.transform.position).normalized;
            offset.Scale(new Vector3(20, 20, 20));
            var deviation = Quaternion.Euler(Random.Range(-30, 30), Random.Range(-30, 30), Random.Range(-30, 30));

            Waypoint = transform.position + (deviation * offset);
        }

        if (!AvoidObstacles) {            
            return;
        }

        var complexityThreshold = 50;
        var hit = false;
        Ray ray;
        var hitInfo = new RaycastHit();
        var lastHit = transform;
        
        do {
            ray = new Ray(transform.position, (Waypoint - transform.position).normalized);
            hit = Physics.Raycast(ray, out hitInfo, (Waypoint - transform.position).magnitude);

            hit = hit && (hitInfo.transform.parent ?? hitInfo.transform).gameObject != Target;

            if (hit) {

                // generate offset
                var hitOffset = hitInfo.point - hitInfo.transform.position;

                var dirToHit = hitOffset.normalized;
                var dirToObj = (hitInfo.transform.position - transform.position).normalized;

                var hitAngle = Mathf.Acos(Vector3.Dot(dirToHit, -dirToObj));
                var hitAxis = Vector3.Cross(dirToHit, -dirToObj).normalized;

                float offsetScale = hitOffset.magnitude + 3.0f;

                if (hitInfo.transform == lastHit) {
                    offsetScale += (Waypoint - hitInfo.point).magnitude * 0.5f;
                    //Debug.Log("NavigationControlSystem::CreateWaypoints() - Re-evaluating: " + hitInfo.transform.gameObject.name);
                } else {
                    //Debug.Log("NavigationControlSystem::CreateWaypoints() - Avoiding: " + hitInfo.transform.gameObject.name);
                }

                var offset = Vector3.Cross(-dirToObj, hitAxis).normalized;
                offset.Scale(new Vector3(offsetScale, offsetScale, offsetScale));

                lastHit = hitInfo.transform;
                Waypoint = hitInfo.point + offset;
            } 
        } while (hit && complexityThreshold-- > 0);
    }

    //private void GenerateWaypoints() {
    //    //Debug.Log("NavigationControlSystem::CreateWaypoints() - BEGIN");

    //    var path = new Stack<Vector3>();
    //    path.Push(transform.position);

    //    if (AvoidObstacles) {

    //        var complexityThreshold = 50;
    //        var hit = false;
    //        var hitInfo = new RaycastHit();
    //        var lastHit = transform;

    //        do {
    //            var ray = new Ray(path.Peek(), (Target.transform.position - path.Peek()).normalized);
    //            hit = Physics.Raycast(ray, out hitInfo, (path.Peek() - Target.transform.position).magnitude);

    //            hit = hit && (hitInfo.transform.parent ?? hitInfo.transform).gameObject != Target;

    //            if (hit) {

    //                // generate offset
    //                var hitOffset = hitInfo.point - hitInfo.transform.position;

    //                var dirToHit = hitOffset.normalized;
    //                var dirToObj = (hitInfo.transform.position - path.Peek()).normalized;

    //                var hitAngle = Mathf.Acos(Vector3.Dot(dirToHit, -dirToObj));
    //                var hitAxis = Vector3.Cross(dirToHit, -dirToObj).normalized;

    //                float offsetScale = hitOffset.magnitude + 3.0f;

    //                if (hitInfo.transform == lastHit) {
    //                    offsetScale += (path.Pop() - hitInfo.point).magnitude * 0.5f;
    //                    //Debug.Log("NavigationControlSystem::CreateWaypoints() - Re-evaluating: " + hitInfo.transform.gameObject.name);
    //                } else {
    //                    //Debug.Log("NavigationControlSystem::CreateWaypoints() - Avoiding: " + hitInfo.transform.gameObject.name);
    //                }

    //                var offset = Vector3.Cross(-dirToObj, hitAxis).normalized;
    //                offset.Scale(new Vector3(offsetScale, offsetScale, offsetScale));

    //                lastHit = hitInfo.transform;
    //                path.Push(hitInfo.point + offset);
    //            }

    //            if (path.Count > 20) {
    //                hit = false;
    //                //Debug.LogWarning("NavigationControlSystem::CreateWaypoints() - Path Too Complex!");

    //            }
    //        } while (hit && --complexityThreshold >= 0);
    //    }

    //    path.Push(Target.transform.position);

    //    // reverse order and transfer to Waypoints stack
    //    Waypoints.Clear();
    //    while (path.Count > 0)
    //        Waypoints.Push(path.Pop());

    //    _CurrentWaypoint = (Waypoints.Count > 0) ? Waypoints.Peek() : new Vector3(float.NaN, float.NaN, float.NaN);
    //    //Debug.Log("NavigationControlSystem::CreateWaypoints() - END");
    //}

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(Waypoint, 0.5f);
    }
}
