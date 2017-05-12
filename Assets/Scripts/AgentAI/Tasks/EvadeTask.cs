using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace AgentAI.Tasks {
    public class EvadeTask : AgentTask {

        public float EscapeDistance = 20;
        public List<string> ThreatTags;

        private NavigationControlSystem NCS;
        
        private GameObject TargetToEvade;

        public override bool CanExit {
            get {
                return (TargetToEvade == null);
            }
        }

        public override float Priority {
            get {
                return (CanExit) ? 0 : MaxPriority;
            }
        }

        public override void Enter() {
            NCS.SetNavTask(TargetToEvade, mode: NavigationControlSystem.NavigationMode.Evade);

        }

        public override void Exit() {
            NCS.SetIdle();
        }

        // Use this for initialization
        void Start() {
            NCS = GetComponent<NavigationControlSystem>();
        }

        // Update is called once per frame
        void Update() {
            if (TargetToEvade == null)
                return;

            if ((TargetToEvade.transform.position - transform.position).magnitude > EscapeDistance)
                TargetToEvade = null;
        }


        void OnScannerDetect(OnboardScanner.Result result) {

            var threats = result.Items;

            var distance = float.PositiveInfinity;
            GameObject closest = null;

            foreach (var threat in threats) {
                if (!ThreatTags.Contains(threat.tag)) continue;

                var d = (threat.transform.position - transform.position).sqrMagnitude;

                if (d < distance) {
                    closest = threat;
                    distance = d;
                }
            }

            if (!float.IsInfinity(distance))
                TargetToEvade = closest;
            else
                TargetToEvade = null;
        }
    }
}