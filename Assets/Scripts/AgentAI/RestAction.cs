using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgentAI.Actions
{
    public class RestAction : Action
    {
        public override void Enter()
        {
            GetComponent<NavigationControlSystem>().IsIdling = true;
        }

        public override float Evaluate()
        {
            return Input.GetKey(KeyCode.Space) ? 1 : 0;
        }

        public override void Exit()
        {
            GetComponent<NavigationControlSystem>().IsIdling = false;
        }

        public override void UpdateAction()
        {

        }
    }
}