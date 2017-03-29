using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class AsteroidGenerator : MonoBehaviour {

    private List<GameObject> AsteroidCache;


    [System.Serializable]
    public class Bounds
    {
        public Vector3 Maxima;
        public Vector3 Minima;
    }

    public GameObject AsteroidCollection;
    public List<GameObject> AsteroidPrefabs;


    public int SpawnLimit;
    public float SpawnDelay;
    public Bounds SpawnArea;
    public Bounds SpawnVelocity;
    
    private float nextSpawn { get; set; }

    // Use this for initialization 
    void Start () {
        nextSpawn = SpawnDelay;
        AsteroidCache = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.realtimeSinceStartup >= nextSpawn) {
            nextSpawn = Time.realtimeSinceStartup + SpawnDelay;

            // Spawn Asteroids
            var spawnPosn = new Vector3();
            spawnPosn.x = Random.Range(SpawnArea.Minima.x, SpawnArea.Maxima.x);
            spawnPosn.y = Random.Range(SpawnArea.Minima.y, SpawnArea.Maxima.y);
            spawnPosn.z = Random.Range(SpawnArea.Minima.z, SpawnArea.Maxima.z);

            var spawnVel = new Vector3();
            spawnVel.x = Random.Range(SpawnVelocity.Minima.x, SpawnVelocity.Maxima.x);
            spawnVel.y = Random.Range(SpawnVelocity.Minima.y, SpawnVelocity.Maxima.y);
            spawnVel.z = Random.Range(SpawnVelocity.Minima.z, SpawnVelocity.Maxima.z);

            var primaryFactor = 2.0f + (1.5f * Mathf.Cos(Random.Range(0.0f, 180.0f)));
            var secondaryFactor = 0.1f * Mathf.Cos(Random.Range(0.0f, 180.0f));

            var spawnScale = new Vector3();
            spawnScale.x = primaryFactor + (0.1f * Mathf.Cos(Random.Range(0.0f, 180.0f)));
            spawnScale.y = primaryFactor + (0.1f * Mathf.Cos(Random.Range(0.0f, 180.0f)));
            spawnScale.z = primaryFactor + (0.1f * Mathf.Cos(Random.Range(0.0f, 180.0f)));

            SpawnAsteroid(spawnPosn, spawnVel, spawnScale);
        }
	}

    public void DespawnAsteroid(GameObject asteroid) {
        asteroid.SetActive(false);
    }

    public GameObject SpawnAsteroid(Vector3 position, Vector3 velocity, Vector3 scale) {
        var asteroid = AsteroidCache.FirstOrDefault(m => !m.activeSelf);
        if (asteroid == null) {
            if (AsteroidCache.Count >= SpawnLimit) return null;
                            
            asteroid = Instantiate(AsteroidPrefabs[Random.Range(0, AsteroidPrefabs.Count)], AsteroidCollection.transform);
            AsteroidCache.Add(asteroid);
            //Debug.Log("Created new Asteroid Object");
        } else {
            //Debug.Log("Re-used Asteroid object");
        }

        asteroid.SetActive(true);
        asteroid.transform.position = position;
        asteroid.transform.localScale = scale;
        asteroid.GetComponent<Rigidbody>().velocity = velocity;

        return asteroid;
    }
}
