using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AgentAI.Actions
{
    public class MoveAction : AgentAction
    {
        private NavigationControlSystem NCS;
        private GameObject Destination;

        public override bool IsComplete
        {
            get
            {
                return NCS.IsArrived;
            }
        }

        public MoveAction(NavigationControlSystem ncs, GameObject destination, float outerRange = 5.0f)
        {
            NCS = ncs;
            Destination = destination;
        }

        public override void Enter()
        {
            NCS.Target = Destination;
            NCS.State = NavigationControlSystem.NavigationState.Seek;
            NCS.StateOnArrival = NavigationControlSystem.NavigationState.Idle;
        }

        public override void Exit()
        {
            NCS.Target = null;
        }

        public override void UpdateAction()
        {
        }
    }
}
