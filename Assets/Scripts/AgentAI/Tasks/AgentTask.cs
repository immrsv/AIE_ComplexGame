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

        protected Queue<AgentAction> Actions = new Queue<AgentAction>();
        protected float stageBegan;

        protected AgentAction CurrentAction { get { return (Actions.Count > 0) ? Actions.Peek() : null; } }

        public abstract float Priority { get; }
        public virtual bool CanExit { get { return true; } }

        public abstract void Enter();
        public abstract void Exit();

<<<<<<< HEAD
        public AgentAction CurrentAction {
            get {
                return (Actions.Count > 0) ? Actions.Peek() : null;
            }
        }
=======
        public bool PrintDebuggingOutput = false;
>>>>>>> 189cac7474d0e284105c8345b16dde050b668048

        public virtual void UpdateTask() {
            if (Actions.Count == 0) return; // No More Actions

<<<<<<< HEAD
            if (Actions.Peek().IsComplete) // If Action is complete, pop from queue
            {
                Actions.Dequeue().Exit();


                if (Actions.Count > 0)
                    Actions.Peek().Enter();
=======
            CurrentAction.PrintDebuggingOutput = PrintDebuggingOutput;

            if (CurrentAction.IsComplete) // If Action is complete, pop from queue
            {
                if (PrintDebuggingOutput)
                    Debug.Log("Ship [" + gameObject.name + "] doing [" + GetType().Name + "] completed action [" + Actions.Peek().GetType().Name + "].");
                
                Actions.Dequeue().Exit();

                if (Actions.Count > 0) {
                    if (PrintDebuggingOutput)
                        Debug.Log("Ship [" + gameObject.name + "] doing [" + GetType().Name + "] completed action [" + Actions.Peek().GetType().Name + "].");

                    CurrentAction.PrintDebuggingOutput = PrintDebuggingOutput;
                    CurrentAction.Enter();
                }
                else {
                    if (PrintDebuggingOutput)
                        Debug.Log("Ship [" + gameObject.name + "] doing [" + GetType().Name + "] completed ALL actions.");
                }
>>>>>>> 189cac7474d0e284105c8345b16dde050b668048

                stageBegan = Time.realtimeSinceStartup;
            }

<<<<<<< HEAD
            if (Actions.Count > 0)
                Actions.Peek().UpdateAction();
        }
=======
            if (CurrentAction != null)
                CurrentAction.UpdateAction();
        }

>>>>>>> 189cac7474d0e284105c8345b16dde050b668048
    }
}