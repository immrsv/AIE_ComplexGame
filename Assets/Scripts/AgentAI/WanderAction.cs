using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgentAI.Actions
{
    public class WanderAction : Action
    {
        public float modifier = 1;
        public float wanderTime = 4.0f;

        private float timeToDestChange = 0f;


        public override void Enter()
        {
            
        }

        public override float Evaluate()
        {
            return modifier * 0.5f;
        }

        public override void Exit()
        {
        }

        public override void UpdateAction()
        {
            if (Time.realtimeSinceStartup > timeToDestChange)
                ChangeDestination();
        }

        private void ChangeDestination()
        {
            timeToDestChange = Time.realtimeSinceStartup + wanderTime;

            Vector3 poi = UnityEngine.Random.insideUnitSphere;
            poi.Scale(new Vector3(50, 50, 50));

            GetComponent<NavigationControlSystem>().NextTarget = poi;
        }
    }
}