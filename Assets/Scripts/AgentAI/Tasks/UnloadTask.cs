using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgentAI.Actions;

namespace AgentAI.Tasks
{
    public class UnloadTask : AgentTask
    {

        public float ResourceTransferRate = 5.0f;
        /// <summary>
        /// Stage in Process to achieve action
        ///  1. Approach Mothership
        ///  2. Request Docking Node (Position)
        ///  3. Approach Docking Node (Position)
        ///  4. "Bind" to Docking Node
        ///  5. Transfer Resources
        /// </summary>
        private Queue<AgentAction> Actions = new Queue<AgentAction>();
        private float stageBegan;

        private GameObject ms;
        private NavigationControlSystem NCS;

        // Use this for initialization
        void Start()
        {
            ms = GameObject.Find("Mothership");
            NCS = GetComponent<NavigationControlSystem>();
        }


        public override void Enter()
        {
            // Rebuild Action queue
            Actions.Clear();
            Actions.Enqueue(new MoveAction(NCS, ms.transform.position));
            Actions.Enqueue(new TransferResourceAction(this.GetComponent<ResourceContainer>(), ms.GetComponent<ResourceContainer>(), ResourceTransferRate));

            stageBegan = Time.realtimeSinceStartup;
        }

        public override float Priority
        {
            get
            {
                var storage = GetComponent<ResourceContainer>();

                // Return 0% need below 50% capacity, raise need quadritically until 90% need at 100% full (allow for Evading Enemies)
                return storage.PercentFull < 0.5f ? 0.0f : 0.9f * Mathf.Pow(storage.PercentFull, 2.0f);
            }
        }

        public override bool CanExit
        {
            get
            {
                if (Actions.Count == 0) return true; // No Actions queued

                var result = true;
                try
                {
                    // Can Exit if Not a TransferResource action, or the action is complete
                    result = !(Actions.Peek() is TransferResourceAction) || Actions.Peek().IsComplete;
                }
                catch (System.InvalidOperationException err)
                {
                    Debug.LogError(GetType().Name + "::CanExit{Get} > " + err.GetType().Name + "::" + err.Message);

                }
                return result;
            }
        }

        public override void Exit()
        {
        }

        public override void UpdateTask()
        {
            if (Actions.Count == 0) return; // No More Actions

            if (Actions.Peek().IsComplete) // If Action is complete, pop from queue
            {
                Debug.Log("Ship [" + gameObject.name + "] completed action [" + Actions.Peek().GetType().Name + "].");
                Actions.Dequeue().Exit();


                if (Actions.Count > 0)
                {
                    Debug.Log("Ship [" + gameObject.name + "] completed action [" + Actions.Peek().GetType().Name + "].");
                    Actions.Peek().Enter();
                }
                else
                {
                    Debug.Log("Ship [" + gameObject.name + "] completed ALL actions.");
                }

                stageBegan = Time.realtimeSinceStartup;
            }

            if ( Actions.Count > 0)
                Actions.Peek().UpdateAction();
        }

    }
}