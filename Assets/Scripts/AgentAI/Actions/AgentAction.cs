using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgentAI.Actions
{
    public abstract class AgentAction
    {
        public abstract bool IsComplete { get; }
        public abstract void UpdateAction();
        public abstract void Enter();
        public abstract void Exit();
    }
}
