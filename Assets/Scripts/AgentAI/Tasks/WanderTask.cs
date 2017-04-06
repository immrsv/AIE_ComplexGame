using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgentAI.Tasks
{
    public class WanderTask : AgentTask
    {
        public float NeedRate = 0.01f;
        public float AttentionSpan = 4.0f;

        private float timeToDestChange = 0f;

        private float NeedValue = 0.0f;

        private NavigationControlSystem NCS;



        public override float Priority
        {
            get
            {
                return Mathf.Min(NeedValue, MaxPriority);
            }
        }

        public override bool CanExit
        {
            get
            {
                return true;
            }
        }

        public void Start()
        {
            NCS = GetComponent<NavigationControlSystem>();
        }

        public void Update()
        {
            NeedValue += NeedRate * Time.deltaTime;
        }

        public override void Enter()
        {
            ChangeDestination();
        }

        public override void Exit()
        {
            NCS.Target = null;

        }

        public override void UpdateTask()
        {
            NeedValue -= UnityEngine.Random.Range(1.5f, 2.5f) * NeedRate * Time.deltaTime;

            if (Time.realtimeSinceStartup > timeToDestChange)
                ChangeDestination();
        }
        
        private void ChangeDestination()
        {
            timeToDestChange = Time.realtimeSinceStartup + AttentionSpan;

            Vector3 poi = UnityEngine.Random.insideUnitSphere;
            poi.Scale(new Vector3(50, 50, 50));

            SetTarget(poi);
        }

        private void SetTarget(Vector3 poi)
        {
            var temp = new GameObject();
            temp.transform.position = poi;

            NCS.Target = temp;
            NCS.State = NavigationControlSystem.NavigationState.Seek;
            NCS.StateOnArrival = NavigationControlSystem.NavigationState.Idle;
        }
    }
}