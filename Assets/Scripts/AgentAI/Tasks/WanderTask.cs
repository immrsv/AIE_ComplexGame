using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgentAI.Tasks
{
    public class WanderTask : AgentTask
    {
        public float NeedValue = 0.5f;
        public float wanderTime = 4.0f;

        private float timeToDestChange = 0f;
        

        public override void Enter()
        {
        }

        public override float Priority
        {
            get
            {
                return NeedValue;
            }
        }

        public override bool CanExit
        {
            get
            {
                return true;
            }
        }

        public override void Exit()
        {
        }

        public override void UpdateTask()
        {
            if (Time.realtimeSinceStartup > timeToDestChange)
                ChangeDestination();
        }

        private void Wander()
        {

        }

        private void ChangeDestination()
        {
            timeToDestChange = Time.realtimeSinceStartup + wanderTime;

            Vector3 poi = UnityEngine.Random.insideUnitSphere;
            poi.Scale(new Vector3(50, 50, 50));

            GetComponent<NavigationControlSystem>().Target = poi;
        }
    }
}