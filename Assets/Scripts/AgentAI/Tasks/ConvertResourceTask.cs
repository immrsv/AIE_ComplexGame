using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Resources;

namespace AgentAI.Tasks {
    public class ConvertResourceTask : AgentTask {

        [System.Serializable]
        public class ProductionItem {
            public ResourceType Type;
            public string Location;
            public float Qty;
        }

        public ProductionItem Produce;
        public List<ProductionItem> Consume = new List<ProductionItem>();

        public float ConversionRate = 1.0f;
        
        public float QtyStarved = 2.0f; // "MIN" quantity to achieve highest priority
        public float QtyGlutted = 10.0f; // "MAX" quantity to maintain

        private ContainerCollection Containers;

        public override float Priority {
            get {
                if (Produce.Location == null || Containers[Produce.Location].GetQuantity(Produce.Type) >= QtyGlutted)
                    return 0.0f; // No Destination Location, or "Glut" Quantity already available, Task has zero priority

                return MaxPriority * (Mathf.SmoothStep(QtyGlutted, QtyStarved, Containers[Produce.Location].GetQuantity(Produce.Type)) / QtyGlutted);
            }
        }

        public new bool CanExit {  get { return Containers[Produce.Location].GetQuantity(Produce.Type) >= QtyStarved; } }

        public override void Enter() {
            
        }

        public override void Exit() {
            
        }

        public new void UpdateTask() {
                DoConversion();
        }

        // Use this for initialization
        void Start() {
            Containers = GetComponent<ContainerCollection>();
        }

        // Update is called once per frame
        void Update() {

        }

        void DoConversion() {
            if (Containers[Produce.Location].GetQuantity(Produce.Type) >= QtyGlutted)
                return; // Do not convert is product is in glut ("MAX" quantity)


            var productionModifier = 1.0f;

            if (Produce.Type != ResourceType._NONE) {
                //  Find smallest of available space or required quantity
                float producableQty = Mathf.Min(QtyGlutted - Containers[Produce.Location].GetQuantity(Produce.Type), Containers[Produce.Location].FreeSpace);

                // Find percentage of Conversion required for the producable quantity
                productionModifier = Mathf.Clamp01(producableQty / ConversionRate);
            }

            // Find percentage of conversion available from ingredients
            foreach (var cost in Consume) {
                // Available Quantity / Required Quantity (cost per unit * production modifier)
                productionModifier = Mathf.Min(productionModifier, Containers[cost.Location].GetQuantity(cost.Type) / (cost.Qty * productionModifier)); 
            }

            // Modify for deltaTime
            productionModifier *= Time.deltaTime;

            // Do conversions
            Containers[Produce.Location].AddContents(Produce.Type, ConversionRate * productionModifier);

            foreach (var cost in Consume) {
                Containers[cost.Location].RemoveContents(cost.Type, cost.Qty * productionModifier);
            }
        }
    }
}