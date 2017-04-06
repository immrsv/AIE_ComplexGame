using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using Resources;

namespace AgentAI.Actions
{
    public class TransferResourceAction : AgentAction
    {
        ResourceContainer Src, Dst;
        float UnitsPerSecond;
        
        public override bool IsComplete { get { return (Src.TotalQuantity <= 0 || Dst.PercentFull >= 1.0f); } }

        public TransferResourceAction(ResourceContainer src, ResourceContainer dst, float unitsPerSecond)
        {
            Src = src;
            Dst = dst;
            UnitsPerSecond = unitsPerSecond;
        }

        public override void Enter()
        {

        }

        public override void Exit()
        {

        }

        public override void UpdateAction()
        {
            if (IsComplete) // Source Empty, or Destination Full!
                return;

            if (Src.Items.Count == 0) return;

            var resource = Src.Items.First().Key;
            var qty = Mathf.Min(Src.Items[resource], UnitsPerSecond * Time.deltaTime);

            if (!Dst.Items.ContainsKey(resource))
                Dst.Items.Add(resource, 0.0f);

            Src.Items[resource] -= qty;
            Dst.Items[resource] += qty;

            if (Src.Items[resource] <= 0)
                Src.Items.Remove(resource);
        }
    }
}
