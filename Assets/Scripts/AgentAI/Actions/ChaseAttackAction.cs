using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgentAI.Actions {
    public class ChaseAttackAction : AgentAction {

        private NavigationControlSystem NCS;
        private GameObject Target;


        public ChaseAttackAction(NavigationControlSystem ncs, GameObject target) {
            NCS = ncs;
            Target = target;
        }

        public override bool IsComplete {
            get {
                return false;
            }
        }

        public override void Enter() {
            
        }

        public override void Exit() {

        }

        public override void UpdateAction() {
            NCS.SetNavTask(Target, 20, NavigationControlSystem.NavigationMode.ContinuousSeek);
        }
        
    }
}