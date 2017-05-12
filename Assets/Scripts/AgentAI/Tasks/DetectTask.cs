using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AgentAI.Tasks {
    public class DetectTask : AgentTask {

        public string TagFilter;

        public float ScanRange = 20;
        public float ScanDelay = 20;
        public float ScanLength = 3;

        private float NextScan;
        private float CanExitAfter;

        private bool HasScanned;

        public override float Priority {
            get {
                return Time.realtimeSinceStartup < NextScan ? 0 : MaxPriority;
            }
        }

        public override bool CanExit {
            get {
                return Time.realtimeSinceStartup >= CanExitAfter;
            }
        }

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        public override void Enter() {
            NextScan = Time.realtimeSinceStartup + ScanDelay + ScanLength;
            CanExitAfter = Time.realtimeSinceStartup + ScanLength;

            DoScan();
        }

        public override void Exit() {
            
        }

        public override void UpdateTask() {
            
        }

        private void DoScan() {
            //var hits = Physics.OverlapSphere(transform.position, 50, LayerMask, QueryTriggerInteraction.Ignore); // Layer 8 is Droneships
            var hits = Physics.OverlapSphere(transform.position, ScanRange);

            var objects = new List<GameObject>();
            foreach (var hit in hits) {
                if (hit.gameObject.tag == TagFilter) {
                    objects.Add(hit.gameObject);
                }
            }

            Debug.Log(gameObject.name + " :: DetectionTask :: " + hits.Length + " hits, " + objects.Count + " objects tagged [" + TagFilter + "]");
            if (hits != null && hits.Length > 0)
                BroadcastMessage("OnScanDetected", objects);
        }
    }
}