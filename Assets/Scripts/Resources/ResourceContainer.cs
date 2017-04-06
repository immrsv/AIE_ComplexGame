using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Resources {
    [System.Serializable]
    public class ResourceContainer {

        public string Name = "Resource Container";

        public Dictionary<ResourceType, float> Items = new Dictionary<ResourceType, float>();

        public float MaxQuantity = 5;
        public float TotalQuantity { get { return Items.Values.Sum<float>(m => m); } }
        public float FreeSpace { get { return MaxQuantity - TotalQuantity; } }
        public float PercentFull { get { return Mathf.Clamp01(TotalQuantity / Mathf.Max(MaxQuantity, 1.0f)); } }


        public float AddContents(ResourceType type, float qty) {
            if (qty < float.Epsilon || type == ResourceType._NONE)
                return 0.0f;

            if (!Items.ContainsKey(type) && qty > 0.0f)
                Items.Add(type, 0.0f);

            var transferQty = Mathf.Min(MaxQuantity - TotalQuantity, qty);

            Items[type] += transferQty;

            return transferQty;
        }

        public float RemoveContents(ResourceType type, float qty) {
            if (!Items.ContainsKey(type) || qty < float.Epsilon)
                return 0.0f; // No Key or transferable quantity, therfore no contents transfered

            var transferQty = Mathf.Min(Items[type], qty);

            Items[type] -= transferQty;

            if (Items[type] < float.Epsilon)
                Items.Remove(type);

            return transferQty;
        }

        public float GetQuantity(ResourceType type) {
            return (Items.ContainsKey(type)) ? Items[type] : 0.0f;
        }
    }

}