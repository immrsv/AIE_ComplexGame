using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgentAI.Tasks {
    public class ScanAsteroidsTask : AgentTask {

        private Dictionary<AsteroidAttributes, float> Asteroids = new Dictionary<AsteroidAttributes, float>();

        private Queue<AsteroidAttributes> ScanQueue = new Queue<AsteroidAttributes>();


        // Percentage of Asteroid scans that are "current"
        private float Completedness { get; set; }

        public override float Priority {
            get {
                return MaxPriority * Mathf.Clamp01(1.0f - Completedness);
            }
        }

        public override bool CanExit {
            get {
                return false;
            }
        }

        public override void Enter() {
            throw new NotImplementedException();
        }

        public override void Exit() {
            throw new NotImplementedException();
        }

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        void UpdateCompletedness() {
            foreach ( var roid in Asteroids ) {

            }
        }
    }
}