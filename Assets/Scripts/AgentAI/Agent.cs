using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgentAI.Tasks;

namespace AgentAI
{
    [DisallowMultipleComponent]
    public class Agent : MonoBehaviour
    {

        AgentTask[] Tasks;

        private AgentTask _CurrentTask;
        public AgentTask CurrentTask { get
            {
                return _CurrentTask;
            } set
            {
                if (_CurrentTask)
                    _CurrentTask.Exit();

                _CurrentTask = value;

                if (_CurrentTask)
                    _CurrentTask.Enter();
            }
        }

        private System.Text.StringBuilder sb;

        // Use this for initialization
        void Start()
        {
            Tasks = GetComponents<AgentTask>();
        }

        // Update is called once per frame
        void Update()
        {
            sb = new System.Text.StringBuilder("Agent:");

            AgentTask best = GetBestTask();

            if (CurrentTask != best && (CurrentTask == null || CurrentTask.CanExit))
            {
                CurrentTask = best;
                Debug.Log("Agent Re-tasked: " + CurrentTask.GetType().ToString());
            }

            if (CurrentTask)
                CurrentTask.UpdateTask();

            GameObject.Find("AgentWatcher").GetComponent<UnityEngine.UI.Text>().text = sb.ToString();
        }
        

        AgentTask GetBestTask()
        {
            AgentTask action = null;
            float bestValue = 0.0f;

            sb.Append("\n=====   Task Priorities   =====");

            foreach(AgentTask a in Tasks)
            {
                float value = a.Priority;
                if (a == CurrentTask) value += 0.1f; // "Commitment"

                sb.Append("\n" + value.ToString("N3") + " : " + a.GetType().Name);

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