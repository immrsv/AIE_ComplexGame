using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgentAI.Actions;

namespace AgentAI.Tasks
{
    public abstract class AgentTask : MonoBehaviour
    {

        protected Queue<AgentAction> Actions = new Queue<AgentAction>();
        protected float stageBegan;

        protected AgentAction CurrentAction { get { return (Actions.Count > 0) ? Actions.Peek() : null; } }

        public abstract float Priority { get; }
        public virtual bool CanExit { get { return true; } }

        public abstract void Enter();
        public abstract void Exit();

        public bool PrintDebuggingOutput = false;

        public virtual void UpdateTask() {
            if (Actions.Count == 0) return; // No More Actions

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

                stageBegan = Time.realtimeSinceStartup;
            }

            if (CurrentAction != null)
                CurrentAction.UpdateAction();
        }

    }
}