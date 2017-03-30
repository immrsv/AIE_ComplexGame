using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgentAI.Actions;

namespace AgentAI.Tasks {
    public class UnloadTask : AgentTask {
        public float MinimumFill = 0.5f;
        public float MaximumPriority = 0.9f;

        public float ResourceTransferRate = 5.0f;

        private GameObject ms;
        private NavigationControlSystem NCS;

        // Use this for initialization
        void Start() {
            ms = GameObject.Find("Mothership");
            NCS = GetComponent<NavigationControlSystem>();
        }

        public override float Priority {
            get {
                var storage = GetComponent<ResourceContainer>();

                // Return 0% need below 50% capacity, raise need quadritically until 90% need at 100% full (allow for Evading Enemies)
                return storage.PercentFull < MinimumFill ? 0.0f : MaximumPriority * Mathf.Pow(storage.PercentFull, 2.0f);
            }
        }

        public new bool CanExit {
            get {
                if (CurrentAction == null) return true; // No Actions queued

                var result = true;
                try {
                    // Can Exit if Not a TransferResource action, or the action is complete
                    result = !(CurrentAction is TransferResourceAction) || CurrentAction.IsComplete;
                } catch (System.InvalidOperationException err) {
                    Debug.LogError(GetType().Name + "::CanExit{Get} > " + err.GetType().Name + "::" + err.Message);

                }
                return result;
            }
        }

        /// <summary>
        /// Stage in Process to achieve action
        ///  1. Approach Mothership
        ///  2. Request Docking Node (Position)
        ///  3. Approach Docking Node (Position)
        ///  4. "Bind" to Docking Node
        ///  5. Transfer Resources
        /// </summary>
        public override void Enter() {
            
            // Rebuild Action queue
            Actions.Clear();
            Actions.Enqueue(new MoveAction(NCS, ms.transform.position));
            Actions.Enqueue(new TransferResourceAction(this.GetComponent<ResourceContainer>(), ms.GetComponent<ResourceContainer>(), ResourceTransferRate));

            stageBegan = Time.realtimeSinceStartup;
        }


        public override void Exit() {
        }


    }
}