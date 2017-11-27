using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidAttributes : MonoBehaviour {

    public bool IsActive { get { return gameObject.activeSelf; } }
    public float SpawnedAt { get; set; }


    private AsteroidGenerator _Generator;
    private AsteroidGenerator Generator {
        get {
            if ( _Generator == null ) {
                _Generator = FindObjectOfType<AsteroidGenerator>();
            }
            return _Generator;
        }
    }

	// Use this for initialization
	void Start () {
        //
    }
	
	// Update is called once per frame
	void Update () {
        var closure = Vector3.Dot(GetComponentInChildren<Rigidbody>().velocity.normalized, transform.position.normalized);
        if (transform.position.magnitude > 100.0f && closure > 0.1f) {
            // Outside Radius and increasing distance, so destroy
            gameObject.SetActive(false);
            Debug.Log("Destroyed Asteroid: Position @ " + transform.position + " , Range @ " + transform.position.magnitude + " , Rate of Closure @ " + closure);
        }
    }

    private void OnEnable() {
        SpawnedAt = Time.realtimeSinceStartup;
    }

    private void OnDisable() {
        if ( Generator != null )
            Generator.DespawnAsteroid(gameObject);
    }

    private void OnCollisionEnter(Collision collision) {

    }
}
