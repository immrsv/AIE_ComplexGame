using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AgentAI.Tasks;

namespace AgentAI
{
    [DisallowMultipleComponent]
    public class Agent : MonoBehaviour
    {

        private class TaskPool {
            public AgentTask[] AllTasks;
            private AgentTask _CurrentTask;
            public float CurrentTaskBegan;

            public AgentTask CurrentTask {
                get {
                    return _CurrentTask;
                }
                set {
                    if (_CurrentTask != null)
                        _CurrentTask.Exit();

                    _CurrentTask = value;

                    Debug.Log("Retasked: " + (_CurrentTask != null ? _CurrentTask.GetType().Name : "<null>"));

                    if (_CurrentTask != null) {
                        _CurrentTask.Enter();
                        CurrentTaskBegan = Time.realtimeSinceStartup;
                    }
                }
            }
        }

        public bool DisplayDebug;

        private Dictionary<uint, TaskPool> TaskPools = new Dictionary<uint, TaskPool>();

        private System.Text.StringBuilder sb;

        public string Log {  get { return sb.ToString(); } }

        // Use this for initialization
        void Start()
        {
            var tasks = GetComponents<AgentTask>().GroupBy(m =>m.TaskPool);

            foreach (var pool in tasks) {
                TaskPools.Add(pool.Key, new TaskPool { AllTasks = pool.ToArray() });
            }
        }

        // Update is called once per frame
        void Update()
        {
            sb = new System.Text.StringBuilder("Agent\t:: " + gameObject.name);

            sb.Append("\n===============================");

            foreach (var kvp in TaskPools.OrderBy(m => m.Key)) {

                var pool = kvp.Value;
                sb.Append("\nPool \t:: " + kvp.Key);
                sb.Append("\n=====    Current Task     =====");

                if (pool.CurrentTask != null) {
                    sb.Append("\n" + pool.CurrentTask.GetType().Name + (!pool.CurrentTask.CanExit ? " [LOCKED]" : ""));
                    sb.Append("\n> " + ((pool.CurrentTask.CurrentAction != null) ? pool.CurrentTask.CurrentAction.GetType().Name : "<no action>"));
                } else {
                    sb.Append("\n<no task>\n > <no action>");
                }

                AgentTask best = GetBestTask(pool);

                if (pool.CurrentTask == null || (pool.CurrentTask != best && pool.CurrentTask.CanExit)) {
                    pool.CurrentTask = best;
                    
                }


                if (pool.CurrentTask)
                    pool.CurrentTask.UpdateTask();

                sb.Append("\n===============================");
            }

            if (DisplayDebug)
                GameObject.Find("TxtSelectionConsole").GetComponent<UnityEngine.UI.Text>().text = sb.ToString();
        }
        

        AgentTask GetBestTask(TaskPool pool)
        {
            AgentTask action = null;
            float bestValue = 0.0f;

            sb.Append("\n=====   Task Priorities   =====");

            foreach(AgentTask a in pool.AllTasks)
            {
                float value = a.Priority;
                if (a == pool.CurrentTask) value += 0.1f; // "Commitment"

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