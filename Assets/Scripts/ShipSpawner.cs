using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ShipSpawner : MonoBehaviour {

    [System.Serializable]
    public class Bounds {
        public Vector3 Maxima;
        public Vector3 Minima;
    }

    public GameObject ShipPrefab;

    public int MaxCount = 5;
    public float InitialDelay;
    public float RepeatDelay;
    public Bounds SpawnArea;

    private float NextSpawn;

	// Use this for initialization
	void Start () {
        NextSpawn = Time.realtimeSinceStartup + InitialDelay;
    }
	
	// Update is called once per frame
	void Update () {
		if ( Time.realtimeSinceStartup > NextSpawn && transform.childCount < MaxCount) {
            NextSpawn = Time.realtimeSinceStartup + RepeatDelay;

            // Spawn Asteroids
            var spawnPosn = new Vector3();
            spawnPosn.x = Random.Range(SpawnArea.Minima.x, SpawnArea.Maxima.x);
            spawnPosn.y = Random.Range(SpawnArea.Minima.y, SpawnArea.Maxima.y);
            spawnPosn.z = Random.Range(SpawnArea.Minima.z, SpawnArea.Maxima.z);

            var enemy = Instantiate(ShipPrefab, transform);
            enemy.SetActive(true);
            enemy.transform.position = spawnPosn;
        }
	}


}
