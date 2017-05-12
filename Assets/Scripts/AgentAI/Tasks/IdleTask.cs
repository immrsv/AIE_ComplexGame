using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgentAI.Tasks {
    public class IdleTask : AgentTask {

        public float IdleTime;

        private float CanExitAfter;

        private NavigationControlSystem NCS;

        private DebugFlag.DebugLevel DebugLevel;

        public override float Priority {
            get {
                return MaxPriority;
            }
        }

        public override bool CanExit {
            get {
                return Time.realtimeSinceStartup > CanExitAfter;
            }
        }

        public override void Enter() {
            CanExitAfter = Time.realtimeSinceStartup + IdleTime;

            if (DebugLevel.HasAny(DebugFlag.DebugLevel.Information))
                Debug.Log(gameObject.name + " began idling");
        }

        public override void Exit() {
        }

        // Use this for initialization
        void Start() {

            NCS = GetComponent<NavigationControlSystem>();

            var debugFlag = GetComponent<DebugFlag>();
            if (debugFlag != null) DebugLevel = debugFlag.Level;
        }

        // Update is called once per frame
        void Update() {

        }
    }
}