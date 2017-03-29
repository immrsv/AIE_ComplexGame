using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgentAI.Actions
{
    public abstract class Action : MonoBehaviour
    {

        public abstract float Evaluate();
        public abstract void UpdateAction();
        public abstract void Enter();
        public abstract void Exit();
    }
}