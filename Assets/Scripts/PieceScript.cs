using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceScript : MonoBehaviour {

	public float lifespan = 3.0f;
	float fadeTimer = 1.0f;
	public int id = 0;
	bool exploding = false;

	// Use this for initialization
	void Start () {
		GetComponentInChildren<Rigidbody> ().detectCollisions = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (exploding) {
			foreach(MeshRenderer render in GetComponentsInChildren<MeshRenderer>())
			{
				Color temp = render.material.color;
				temp.a = fadeTimer;
				render.material.color = temp;
			}
			fadeTimer -= Time.deltaTime / lifespan;
			if (fadeTimer <= 0.0f) {
				Destroy (this.gameObject);
			}
		}
	}

	public void Explode()
	{
		exploding = true;
		GetComponentInChildren<Rigidbody>().isKinematic = false;
		GetComponentInChildren<Rigidbody> ().detectCollisions = true;
		GetComponentsInChildren<Rigidbody> ()[0].AddForce(new Vector3 (Random.Range (-300, 300), Random.Range (-300, 300), Random.Range (-300, 300)));
		GetComponentsInChildren<Rigidbody> ()[0].AddTorque(new Vector3 (Random.Range (-300, 300), Random.Range (-300, 300), Random.Range (-300, 300)));
		//GetComponent<Rigidbody>().AddExplosionForce(500.0f, new Vector3(Random.Range(2, 20), Random.Range(2, 20), Random.Range(2, 20)), 2.0f);
	}
}
