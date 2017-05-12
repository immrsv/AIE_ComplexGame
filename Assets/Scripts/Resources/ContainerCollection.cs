using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Resources {
    [DisallowMultipleComponent]
    public class ContainerCollection : MonoBehaviour {

        public List<ResourceContainer> Containers = new List<ResourceContainer> { new ResourceContainer() };

        public ResourceContainer this[string name] {
            get {
                if (Containers.Count == 0 || string.IsNullOrEmpty(name))
                    return Containers.FirstOrDefault();

                return Containers.FirstOrDefault(m => m.Name.Trim() == name.Trim());
            }
        }

        public string StatusAsString() {
            var sb = new System.Text.StringBuilder();

            foreach ( var container in Containers) {
                sb.AppendLine("Container: " + container.Name);
                foreach ( var item in container.Items) {
                    sb.AppendLine("> " + item.Key + ": \t" + item.Value.ToString("n2"));
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}