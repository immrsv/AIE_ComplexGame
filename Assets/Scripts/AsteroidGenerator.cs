using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidGenerator : MonoBehaviour {

    [System.Serializable]
    public class Bounds
    {
        public Vector3 Maxima;
        public Vector3 Minima;
    }

    
    public GameObject AsteroidPrefab;

    public Bounds SpawnArea;

    public int SpawnLimit;

    public Bounds SpawnVelocity;

    public float SpawnDelay;


    private float nextSpawn { get; set; }

    
	// Use this for initialization 
	void Start () {
	    	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
