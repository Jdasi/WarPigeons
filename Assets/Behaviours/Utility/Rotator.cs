using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotation_speed = -50;
    public Vector3 rotation_axis = Vector3.up;

	void Update()
    {
		transform.Rotate(rotation_axis.normalized * Time.deltaTime * rotation_speed);
    }
}
