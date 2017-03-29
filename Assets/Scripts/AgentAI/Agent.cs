using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgentAI.Actions;

namespace AgentAI
{
    [DisallowMultipleComponent]
    public class Agent : MonoBehaviour
    {

        Action[] Actions;

        public Action CurrentAction;

        // Use this for initialization
        void Start()
        {
            Actions = GetComponents<Action>();
        }

        // Update is called once per frame
        void Update()
        {
            Action best = GetBestAction();

            if ( CurrentAction != best)
            {
                if (CurrentAction)
                    CurrentAction.Exit();

                CurrentAction = best;

                if (CurrentAction)
                    CurrentAction.Enter();
            }

            if (CurrentAction) CurrentAction.UpdateAction();
        }

        Action GetBestAction()
        {
            Action action = null;
            float bestValue = 0.0f;

            foreach(Action a in Actions)
            {
                float value = a.Evaluate();
                if ( action == null || value > bestValue)
                {
                    action = a;
                    bestValue = value;
                }
            }

            return action;
        }
    }
}