using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ResourceContainer : MonoBehaviour {

    public enum ResourceType
    {
        Ore,
        Reactant,
        Data,
        Metal,
        Fuel,
        Energy,
        Science,
        Shield,
        Health,
    }

    public Dictionary<ResourceType, float> Items;

    

    public float MaxQuantity = 5;
    public float TotalQuantity { get { return Items.Values.Sum<float>(m => m); } }
    public float PercentFull {  get { return Mathf.Clamp01(TotalQuantity / Mathf.Max(MaxQuantity, 1.0f)); } }



    public void Start()
    {
        Items = new Dictionary<ResourceType, float>();       
        
    }

    public void Update()
    {

    }
}
