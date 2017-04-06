using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgentAI.Actions;
using Resources;

<<<<<<< HEAD
namespace AgentAI.Tasks
{
    public class AcquireResourceTask : AgentTask
    {
        public ResourceType Resource;
        public float TransferRate = 1.0f;

        public GameObject Mothership;
        public NavigationControlSystem NCS;

        private GameObject target;
        private ResourceContainer TransferTo;

        private float storageModifier;
        

        public override float Priority
        {
            get
            {
=======
namespace AgentAI.Tasks {
    public class AcquireResourceTask : AgentTask {
        public ResourceContainer.ResourceType Resource;
        public float ResourceTransferRate = 1.0f;


        private GameObject ms;
        private NavigationControlSystem NCS;

        private GameObject target;
        

        public override float Priority {
            get {
>>>>>>> 189cac7474d0e284105c8345b16dde050b668048
                // if has target and target storage is empty, return 0% need (nothing to do here)
                if (target != null && target.GetComponent<ContainerCollection>()[null].TotalQuantity == 0) return 0;

                // Reduce this need by how full our storage is
                storageModifier = 1.0f - Mathf.Pow(TransferTo.PercentFull, 2.0f);

                float result = 0.0f;

                try {
                    // Echo Mothership's Need for this resource (if already transferring resources, ignore storage modifier)
<<<<<<< HEAD
                    result = MaxPriority * ((CurrentAction is TransferResourceAction) ? 1.0f : storageModifier);
                }
                catch (System.InvalidOperationException err)
                {
=======
                    result = 0.8f * ((CurrentAction is TransferResourceAction) ? 1.0f : storageModifier);
                } catch (System.InvalidOperationException err) {
>>>>>>> 189cac7474d0e284105c8345b16dde050b668048
                    Debug.LogError(GetType().Name + "::Priority{Get} > " + err.GetType().Name + "::" + err.Message);
                }

                return result;
            }
        }

<<<<<<< HEAD

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



        public override void Exit()
        {
        }

        public override void UpdateTask()
        {
            if (Actions.Count == 0) return; // No More Actions

            if (Actions.Peek().IsComplete) // If Action is complete, pop from queue
            {
                Actions.Dequeue().Exit();

                if (Actions.Count > 0)
                    Actions.Peek().Enter();
=======

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
>>>>>>> 189cac7474d0e284105c8345b16dde050b668048


        public override void Exit() {
        }
    }
}