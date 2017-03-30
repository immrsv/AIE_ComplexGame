using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgentAI.Actions;

namespace AgentAI.Tasks {
    public class AcquireResourceTask : AgentTask {
        public ResourceContainer.ResourceType Resource;
        public float ResourceTransferRate = 1.0f;


        private GameObject ms;
        private NavigationControlSystem NCS;

        private GameObject target;
        

        public override float Priority {
            get {
                // if has target and target storage is empty, return 0% need (nothing to do here)
                if (target != null && target.GetComponent<ResourceContainer>().TotalQuantity == 0) return 0;

                // Reduce this need by how full our storage is
                var storageModifier = 1.0f - Mathf.Pow(GetComponent<ResourceContainer>().PercentFull, 2.0f);

                float result = 0.0f;

                try {
                    // Echo Mothership's Need for this resource (if already transferring resources, ignore storage modifier)
                    result = 0.8f * ((CurrentAction is TransferResourceAction) ? 1.0f : storageModifier);
                } catch (System.InvalidOperationException err) {
                    Debug.LogError(GetType().Name + "::Priority{Get} > " + err.GetType().Name + "::" + err.Message);
                }

                return result;
            }
        }


        // Use this for initialization
        void Start() {
            ms = GameObject.Find("Mothership");
            NCS = GetComponent<NavigationControlSystem>();
        }

        /// <summary>
        /// Stage in Process to achieve action
        ///  1. Identify Resource Object [On Enter]
        ///  2. Approach Resource Object [As Action]
        ///  3. "Mine" Object (Transfer Resources) [As Action]
        /// </summary>
        public override void Enter() {
            // Identify Target to Acquire From
            target = GameObject.Find("Asteroid 01");

            // Rebuild Action queue
            Actions.Clear();
            Actions.Enqueue(new MoveAction(NCS, target.transform.position));
            Actions.Enqueue(new TransferResourceAction(target.GetComponent<ResourceContainer>(), this.GetComponent<ResourceContainer>(), ResourceTransferRate));

            stageBegan = Time.realtimeSinceStartup;
        }


        public override void Exit() {
        }
    }
}