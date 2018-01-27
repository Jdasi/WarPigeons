using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMap : MonoBehaviour {

	public Transform[] spawnPoints;
	List<bool> playerOwned;
	List<PieceScript> pieces;
	bool spawned = false;
	public GameObject ally, enemy;
	public int soldierSpawn = 3;
	public int doThing = 0;
	int change = -1;
	public int advanced = 1;

	// Use this for initialization
	void Start () {
		playerOwned = new List<bool>();
		for (int i = 0; i < spawnPoints.Length; i++) {
			playerOwned.Add (false);
		}
		playerOwned [0] = true;
		soldierSpawn = Mathf.Max (soldierSpawn, 1);
		soldierSpawn = Mathf.Min (soldierSpawn, 8);
		SpawnObjects ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Advance()
	{
		if (advanced >= playerOwned.Count) {
			advanced = playerOwned.Count - 1;
		}
		if (advanced < 0) {
			advanced = 0;
		}
		CapturePos (advanced);
	}

	public void Retreat()
	{
		if (advanced >= playerOwned.Count) {
			advanced = playerOwned.Count - 1;
		}
		if (advanced < 0) {
			advanced = 0;
		}
		LosePos (advanced);
	}

	void CapturePos(int pos)
	{
		if (!playerOwned [pos]) {
			playerOwned [pos] = true;
			change = pos;
			SpawnObjects ();
			advanced = pos + 1;
		}
		else if(pos < playerOwned.Count - 1)
		{
			pos++;
			playerOwned [pos] = true;
			change = pos;
			SpawnObjects ();
			advanced = pos + 1;
		}
	}

	void LosePos(int pos)
	{
		if (playerOwned [pos]) {
			playerOwned [pos] = false;
			change = pos;
			SpawnObjects ();
			advanced = pos - 1;
		}
		else if(advanced > 0)
		{
			pos--;
			playerOwned [pos] = false;
			change = pos;
			SpawnObjects ();
			advanced = pos - 1;
		}
	}

	void PieceUpdate(int pos)
	{
		playerOwned [pos] = !playerOwned [pos];
		change = pos;
		SpawnObjects ();
	}

	void SpawnObjects()
	{
		if (spawned)
		{
			PieceScript[] pieces = GameObject.FindObjectsOfType<PieceScript> ();
			foreach (PieceScript piece in pieces) {
				if (piece.id == change) {
					piece.Explode ();
				}
			}
			if (change != -1) {
				SpawnPosition (change);
			}
		}
		else
		{
			for (int i = 0; i < playerOwned.Count; i++)
			{
				SpawnPosition (i);
			}
			spawned = true;
		}
	}

	void SpawnPosition(int pos)
	{
		for (int j = 0; j < soldierSpawn; j++) {
			float offsetX, offsetY, angle;
			offsetX = 0.0f;
			offsetY = 0.0f;
			if (j > 0) {
				angle = 6.28319f / (soldierSpawn - 1);
				offsetX = Mathf.Sin (j * angle) * 0.075f;
				offsetY = Mathf.Cos (j * angle) * 0.075f;
			}
			GameObject newObject;
			if (playerOwned [pos]) {
				newObject = Instantiate (ally);
			} else {
				newObject = Instantiate (enemy);
			}
			newObject.transform.position = spawnPoints [pos].position;
			Vector3 newPos = newObject.transform.position;
			newPos.x += offsetX;
			newPos.z += offsetY;
			newObject.transform.position = newPos;
			newObject.GetComponent<PieceScript> ().id = pos;
			//newObject.GetComponent<PieceScript> ().Explode ();
		}
	}

}
