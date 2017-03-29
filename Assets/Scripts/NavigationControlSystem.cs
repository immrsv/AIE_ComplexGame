using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class NavigationControlSystem : MonoBehaviour {

    public List<GameObject> Checkpoints;
    
    public float TargetOuterRadius = 7.0f;
    public float TargetInnerRadius = 2.0f;

    public int LapsRemaining = 10;

    public bool Run = false;

    private int nextTarget = 0;


    
    private FlightControlSystem FCS;

	// Use this for initialization
	void Start () {
        FCS = GetComponent<FlightControlSystem>();
	}
	
	
	void Update () {
        if (Checkpoints.Count == 0) return; // Nothing to see here

        if (IsTargetReached()) SelectNextTarget();
        if (!HasNextTarget()) {
            if (!BeginLap()) {
                DoHoldingPattern();
                return;
            }
        }
        
        var targetPosn = Checkpoints[nextTarget].transform.position;
        var targetDirn = targetPosn - transform.position;

        var targetLocalDirn = transform.InverseTransformDirection(targetDirn);
        var targetLocalRotn = (Quaternion.FromToRotation(Vector3.forward, targetLocalDirn)).eulerAngles;
        targetLocalRotn.Scale(new Vector3(0.2f, 0.2f, 0.2f));

        var desiredMotion = new PhysicsIntent {
            Linear = targetLocalDirn,
            Angular = targetLocalRotn
        };

        FCS.DesiredMotion = desiredMotion;
	}

    void DoHoldingPattern() {
        FCS.DesiredMotion = new PhysicsIntent();
    }
    bool IsTargetReached() {
        if (nextTarget == -1) return false;

        var targetDistance = (transform.position - Checkpoints[nextTarget].transform.position).magnitude;

        if (targetDistance > TargetOuterRadius) return false;
        if (targetDistance < TargetInnerRadius) return true;
        
        var cognitionBand = TargetOuterRadius - TargetInnerRadius;
        var cognitionThreshold = Mathf.Clamp(targetDistance - TargetInnerRadius, 0, cognitionBand) / cognitionBand;
        

        var cognitionChance = Mathf.SmoothStep(0, 1, cognitionThreshold);
        var cognitionRoll = Mathf.Pow(Random.Range(0.0f, 1.0f), 8);
        var cognitionResult = cognitionRoll >= cognitionChance;

        //Debug.Log("Cognition Test: Threshold @ " + cognitionThreshold.ToString("N3") + " , Chance @ " + cognitionChance.ToString("N3") + " , Roll @ " + cognitionRoll.ToString("N3"));

        //if (cognitionResult) Debug.Log("Achieved Target at Distance: " + targetDistance);

        return cognitionResult;
    }

    void SelectNextTarget() {
        if (++nextTarget >= Checkpoints.Count)
            nextTarget = -1;
    }

    bool HasNextTarget() {
        return nextTarget >= 0;
    }

    bool BeginLap() {
        if (LapsRemaining > 0) {
            LapsRemaining--;
            nextTarget = 0;
            return true;
        }
        return false;
    }
}
