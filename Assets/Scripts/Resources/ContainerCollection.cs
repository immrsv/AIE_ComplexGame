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
    }
}