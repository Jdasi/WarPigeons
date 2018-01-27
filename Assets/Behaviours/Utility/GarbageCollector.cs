using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageCollector : MonoBehaviour
{
	[SerializeField] float destroy_delay = 3.0f;


    void Start()
    {
	    Destroy(this.gameObject, destroy_delay);
    }

}
