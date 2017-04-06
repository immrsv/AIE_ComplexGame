using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class NavigationControlSystem : MonoBehaviour {
    
    public enum NavigationState
    {
        Idle,
        Seek,
        Evade,
        Orbit
    }

    public float TargetOuterRadius = 7.0f;
    public float TargetInnerRadius = 2.0f;

<<<<<<< HEAD
    public Collider PersonalSpace;
=======
    public bool IsIdling = true;
    public Vector3? _Target;
    public Vector3? Target { get { return _Target; } set { _Target = value; IsArrived = false; } }
>>>>>>> 189cac7474d0e284105c8345b16dde050b668048

    public GameObject Target;

    public NavigationState State;
    public NavigationState StateOnArrival;

    private FlightControlSystem FCS;

    public bool IsArrived
    {
        get
        {
            if (Target == null ) return true; // No Target

            var targetDistance = (transform.position - Target.transform.position).magnitude;

            if (targetDistance > TargetOuterRadius) return false;
            if (targetDistance < TargetInnerRadius) return true;

            var cognitionBand = TargetOuterRadius - TargetInnerRadius;
            var cognitionThreshold = Mathf.Clamp(targetDistance - TargetInnerRadius, 0, cognitionBand) / cognitionBand;


            var cognitionChance = Mathf.SmoothStep(0, 1, cognitionThreshold);
            var cognitionRoll = Mathf.Pow(Random.Range(0.0f, 1.0f), 8);
            var cognitionResult = cognitionRoll >= cognitionChance;

            return cognitionResult;
        }
    }


	// Use this for initialization
	void Start () {
        FCS = GetComponent<FlightControlSystem>();

        
	}
	
    public void OnTriggerEnter( Collider other  ) {
        if (Target == null) return; // Target is not a defined object
        if (Target.transform != other.transform.root) return; // Collided with something other than target, not relevant here

        Debug.Log(gameObject.name + " Arrived At (Proximity Collider) : " + other.transform.root.name);
        // Arrived
        DoArrived();
    }

	
	void Update () {
        
        if (IsArrived)
        {
            DoArrived();
        }


        switch (State)
        {
            case NavigationState.Idle:
                DoIdle();
                break;
            case NavigationState.Seek:
                DoSeek();
                break;
            case NavigationState.Evade:
                DoEvade();
                break;
            case NavigationState.Orbit:
                DoOrbit();
                break;
        }
	}

    private void DoIdle() {
        FCS.DesiredMotion = new PhysicsIntent();
    }

    private void DoSeek()
    {
        var desiredMotion = new PhysicsIntent();

        var targetPosn = Target.transform.position;
        var targetDirn = targetPosn - transform.position;

        var targetLocalDirn = transform.InverseTransformDirection(targetDirn);
        var targetLocalRotn = (Quaternion.FromToRotation(Vector3.forward, targetLocalDirn)).eulerAngles;
        targetLocalRotn.Scale(new Vector3(0.2f, 0.2f, 0.2f));

        desiredMotion.Linear = targetLocalDirn;
        desiredMotion.Angular = targetLocalRotn;

        FCS.DesiredMotion = desiredMotion;
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
        Target = null;
        State = StateOnArrival;
    }
}
