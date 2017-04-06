using UnityEngine;
using System.Collections;

public class AsteroidMotion : MonoBehaviour 
{
	public float tumble;
	
	void Start ()
	{
        GetComponentInChildren<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble;
	}

    public void Update() {
        var closure = Vector3.Dot(GetComponentInChildren<Rigidbody>().velocity.normalized, transform.position.normalized);
        //if ( transform.position.magnitude > 100.0f && closure > 0.1f ) {
        //    // Outside Radius and increasing distance, so destroy
        //    FindObjectOfType<AsteroidGenerator>().DespawnAsteroid(gameObject);
        //    Debug.Log("Destroyed Asteroid: Position @ " + transform.position + " , Range @ " + transform.position.magnitude + " , Rate of Closure @ " + closure);
        //}
    }
}