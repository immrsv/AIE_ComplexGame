using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgentAI.Actions;

<<<<<<< HEAD
using Resources;

namespace AgentAI.Tasks
{
    public class UnloadTask : AgentTask
    {

        public float TransferRate = 5.0f;
        

        public GameObject Mothership;
        public NavigationControlSystem NCS;

        private ResourceContainer TransferFrom;
        private ResourceContainer TransferTo;

        public override float Priority
        {
            get
            {
                //if (storage.PercentFull < 0.5f)
                //    return 0.0f;
                //else
                //    return 0.9f * Mathf.Pow(storage.PercentFull, 2.0f);

                // Return 0 % need below 50 % capacity, raise need geometricly until 90 % need at 100 % full(allow for Evading Enemies)
                return (TransferFrom.PercentFull < 0.5f) ? 0.0f : (MaxPriority * Mathf.Pow(TransferFrom.PercentFull, 2.0f));
=======
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
>>>>>>> 189cac7474d0e284105c8345b16dde050b668048
            }
        }

        public new bool CanExit {
            get {
                if (CurrentAction == null) return true; // No Actions queued

                var result = true;
                try {
                    // Can Exit if Not a TransferResource action, or the action is complete
<<<<<<< HEAD
                    result = !(CurrentAction is TransferResourceAction) || Actions.Peek().IsComplete;
                }
                catch (System.InvalidOperationException err)
                {
=======
                    result = !(CurrentAction is TransferResourceAction) || CurrentAction.IsComplete;
                } catch (System.InvalidOperationException err) {
>>>>>>> 189cac7474d0e284105c8345b16dde050b668048
                    Debug.LogError(GetType().Name + "::CanExit{Get} > " + err.GetType().Name + "::" + err.Message);

                }
                return result;
            }
        }

<<<<<<< HEAD
        // Use this for initialization
        void Start()
        {

            if ( Mothership == null ) Mothership = GameObject.Find("Mothership");
            if (NCS == null ) NCS = GetComponent<NavigationControlSystem>();

            TransferFrom = GetComponent<ContainerCollection>()["CargoBay"];
            TransferTo = Mothership.GetComponent<ContainerCollection>()["CargoBay"];
        }


        public override void Enter()
        {
            /// <summary>
            /// Stage in Process to achieve action
            ///  1. Approach Mothership
            ///  2. Request Docking Node (Position)
            ///  3. Approach Docking Node (Position)
            ///  4. "Bind" to Docking Node
            ///  5. Transfer Resources
            /// </summary>
            /// 
            // Rebuild Action queue
            Actions.Clear();
            Actions.Enqueue(new MoveAction(NCS, Mothership, 25.0f));
            Actions.Enqueue(new TransferResourceAction(TransferFrom, TransferTo, TransferRate));


            Actions.Peek().Enter();
            stageBegan = Time.realtimeSinceStartup;
        }



        public override void Exit()
        {
=======
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
>>>>>>> 189cac7474d0e284105c8345b16dde050b668048
        }


    }
}