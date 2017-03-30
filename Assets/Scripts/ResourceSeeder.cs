using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSeeder : MonoBehaviour {


    
    public float seedDelay = 10;
    public List<ResourceContainer.ResourceType> Seeds;

    private float nextSeed;
    private ResourceContainer container;

    // Use this for initialization
    void Start () {
        container = GetComponent<ResourceContainer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.realtimeSinceStartup >= nextSeed)
        {
            nextSeed = Time.realtimeSinceStartup + seedDelay;
            foreach (var resource in Seeds)
            {
                container.Items[resource] = container.MaxQuantity / (float)Seeds.Count;
            }
        }
    }
}
