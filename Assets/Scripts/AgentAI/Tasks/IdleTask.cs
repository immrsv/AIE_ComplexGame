using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgentAI.Tasks {
    public class IdleTask : AgentTask {

        public override float Priority {
            get {
                return MaxPriority;
            }
        }

        public override void Enter() {
        }

        public override void Exit() {
        }

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
    }
}