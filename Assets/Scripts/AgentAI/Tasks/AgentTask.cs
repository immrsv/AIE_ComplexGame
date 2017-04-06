using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgentAI.Actions;

namespace AgentAI.Tasks {
    public abstract class AgentTask : MonoBehaviour {

        public uint TaskPool;

        public float MaxPriority = 1.0f;

        protected Queue<AgentAction> Actions = new Queue<AgentAction>();
        protected float stageBegan;

        public abstract float Priority { get; }
        public virtual bool CanExit { get { return true; } }

        public abstract void Enter();
        public abstract void Exit();

        public AgentAction CurrentAction {
            get {
                return (Actions.Count > 0) ? Actions.Peek() : null;
            }
        }

        public virtual void UpdateTask() {
            if (Actions.Count == 0) return; // No More Actions

            if (Actions.Peek().IsComplete) // If Action is complete, pop from queue
            {
                Actions.Dequeue().Exit();


                if (Actions.Count > 0)
                    Actions.Peek().Enter();

                stageBegan = Time.realtimeSinceStartup;
            }

            if (Actions.Count > 0)
                Actions.Peek().UpdateAction();
        }
    }
}