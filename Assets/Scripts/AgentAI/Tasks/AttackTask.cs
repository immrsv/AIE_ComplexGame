using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AgentAI.Actions;

namespace AgentAI.Tasks {
    public class AttackTask : AgentTask {


        public float AttackTimer = 10;
        public float Downtime = 20;

        public List<string> PreyTags;

        private float AttackEnds;
        private float DowntimeEnds;


        public GameObject Target { get; set; }

        private NavigationControlSystem NCS;


        public override float Priority {
            get {
                if (Target == null)
                    return 0;

                if (Time.realtimeSinceStartup >= AttackEnds)
                    return 0;

                return MaxPriority;
            }
        }

        public override bool CanExit {
            get {
                return true;
            }
        }

        public override void Enter() {
            AttackEnds = Time.realtimeSinceStartup + AttackTimer;

            /// <summary>
            /// Stage in Process to achieve action
            ///  1. Navigate to Target
            ///  2. Shoot Target until 'dead'
            /// </summary>
            Actions.Clear();
            Actions.Enqueue(new MoveAction(NCS, Target));
            Actions.Enqueue(new ChaseAttackAction(NCS, Target));

            Actions.Peek().Enter();
            stageBegan = Time.realtimeSinceStartup;
        }

        public override void Exit() {
            Target = null;
            DowntimeEnds = Time.realtimeSinceStartup + Downtime;
            AttackEnds = float.PositiveInfinity;

            NCS.SetIdle();
        }

        // Use this for initialization
        void Start() {
            NCS = GetComponent<NavigationControlSystem>();
            AttackEnds = float.PositiveInfinity;
        }

        // Update is called once per frame
        void Update() {

        }
        

        void OnScannerDetect(OnboardScanner.Result result) {
            if (Time.realtimeSinceStartup < DowntimeEnds)
                return; // TOO SOON! ;)

            var distance = float.PositiveInfinity;
            GameObject closest = null;

            foreach(var obj in result.Items) {
                if (!PreyTags.Contains(obj.tag)) continue;

                var d = (obj.transform.position - transform.position).sqrMagnitude;

                if ( d < distance) {
                    closest = obj;
                    distance = d;
                }
            }

            if (!float.IsInfinity(distance)) Target = closest;
        }
    }
}