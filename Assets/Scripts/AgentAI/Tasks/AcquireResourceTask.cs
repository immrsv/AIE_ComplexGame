using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgentAI.Actions;
using Resources;
using System;

namespace AgentAI.Tasks
{
    public class AcquireResourceTask : AgentTask
    {
        public ResourceType Resource;
        public float TransferRate = 1.0f;

        public GameObject Mothership;
        public NavigationControlSystem NCS;

        private GameObject target;
        private ResourceContainer targetContainer;
        private ResourceContainer TransferTo;

        private float storageModifier;
        

        public override float Priority
        {
            get
            {

                // if has target and target storage is empty, return 0% need (nothing to do here)
                if (target != null && targetContainer.TotalQuantity == 0) return 0;

                // Reduce this need by how full our storage is
                storageModifier = 1.0f - Mathf.Pow(TransferTo.PercentFull, 2.0f);

                float result = 0.0f;

                try {
                    // Echo Mothership's Need for this resource (if already transferring resources, ignore storage modifier)

                    result = MaxPriority * ((CurrentAction is TransferResourceAction) ? 1.0f : storageModifier);
                }
                catch (System.InvalidOperationException err)
                {

                    Debug.LogError(GetType().Name + "::Priority{Get} > " + err.GetType().Name + "::" + err.Message);
                }

                return result;
            }
        }

        public override bool CanExit {
            get {
                return true;
            }
        }
        // Use this for initialization
        void Start()
        {
            if (Mothership == null)
                Mothership = GameObject.Find("Mothership");

            if (NCS == null)
                NCS = GetComponent<NavigationControlSystem>();
            
            TransferTo = GetComponent<ContainerCollection>()["CargoBay"];
        }


        public override void Enter()
        {
            // Identify Target to Acquire From
            target = GameObject.Find("Asteroid 01");

            if (target)
                targetContainer = target.GetComponent<ContainerCollection>()[null];

            /// <summary>
            /// Stage in Process to achieve action
            ///  1. Identify Resource Object [On Enter]
            ///  2. Approach Resource Object [As Action]
            ///  3. "Mine" Object (Transfer Resources) [As Action]
            /// </summary>
            // Rebuild Action queue
            Actions.Clear();
            Actions.Enqueue(new MoveAction(NCS, target));
            Actions.Enqueue(new TransferResourceAction(target.GetComponent<ContainerCollection>()[null], TransferTo, TransferRate));

            Actions.Peek().Enter();
            stageBegan = Time.realtimeSinceStartup;
        }


        public override void UpdateTask() {
            if (Actions.Count == 0) return; // No More Actions

            if (CurrentAction.IsComplete) // If Action is complete, pop from queue
            {
                Actions.Dequeue().Exit();

                if (Actions.Count > 0)
                    Actions.Peek().Enter();
            }

            if (CurrentAction != null)
                CurrentAction.UpdateAction();
        }



        public override void Exit() {
        }
    }
}