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
        private Vector3 Destination;

        public override bool IsComplete { get { return NCS.IsArrived; } }

        public MoveAction(NavigationControlSystem ncs, Vector3 destination)
        {
            NCS = ncs;
            Destination = destination;
        }

        public override void Enter()
        {
            NCS.Target = Destination;
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
