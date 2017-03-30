using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class NavigationControlSystem : MonoBehaviour {
    
    
    public float TargetOuterRadius = 7.0f;
    public float TargetInnerRadius = 2.0f;

    public bool IsIdling = true;
    public Vector3? _Target;
    public Vector3? Target { get { return _Target; } set { _Target = value; IsArrived = false; } }

    public bool IsArrived { get; protected set; }

    private FlightControlSystem FCS;

	// Use this for initialization
	void Start () {
        FCS = GetComponent<FlightControlSystem>();
	}
	
	
	void Update () {
        var desiredMotion = new PhysicsIntent();

        if (!IsArrived)
        {
            IsArrived = IsTargetReached();
            if (!IsIdling)
            {
                var targetPosn = Target.Value;
                var targetDirn = targetPosn - transform.position;

                var targetLocalDirn = transform.InverseTransformDirection(targetDirn);
                var targetLocalRotn = (Quaternion.FromToRotation(Vector3.forward, targetLocalDirn)).eulerAngles;
                targetLocalRotn.Scale(new Vector3(0.2f, 0.2f, 0.2f));

                desiredMotion.Linear = targetLocalDirn;
                desiredMotion.Angular = targetLocalRotn;
            }

            FCS.DesiredMotion = desiredMotion;
        }
	}

    void DoHoldingPattern() {
        FCS.DesiredMotion = new PhysicsIntent();
    }

    bool IsTargetReached() {
        if (!Target.HasValue) return true;

        var targetDistance = (transform.position - Target.Value).magnitude;

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
