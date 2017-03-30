using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgentAI.Tasks
{
    public abstract class AgentTask : MonoBehaviour
    {

        public abstract float Priority { get; }
        public abstract bool CanExit { get; }

        public abstract void UpdateTask();
        public abstract void Enter();
        public abstract void Exit();
    }
}